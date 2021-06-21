using bgTeam;
using bgTeam.Extensions;
using bgTeam.Queues;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PartyMaker.Common.Impl.QueueProvider
{
    public abstract class BaseQueueProviderRabbitMQ : IQueueProvider
    {
        private static readonly object _locker = new object();
        private static readonly object _lockChannel = new object();

        private IMessageProvider _msgInnerProvider;

        private bool _disposed = false;

        private List<string> _queues;
        private IAppLogger _logger;
        private IConnectionFactory _factory;

        private IModel _channel;

        protected BaseQueueProviderRabbitMQ(
            IAppLogger logger,
            IMessageProvider msgProvider,
            IConnectionFactory factory)
        {
            _logger = logger;
            _msgInnerProvider = msgProvider;
            _factory = factory;

            _queues = new List<string>();
        }

        ~BaseQueueProviderRabbitMQ()
        {
            Dispose(false);
        }

        protected IMessageProvider MsgInnerProvider => _msgInnerProvider;

        public virtual void PushMessage(IQueueMessage message)
        {
            PushMessageInternal(_queues, message);
        }

        public virtual void PushMessage(IQueueMessage message, params string[] queues)
        {
            queues = GetDistinctQueues(queues);
            PushMessageInternal(queues, message);
        }

        public uint GetQueueMessageCount(string queueName)
        {
            var queue = _queues.SingleOrDefault(x => x.Equals(queueName, StringComparison.InvariantCultureIgnoreCase));
            if (queue == null)
            {
                throw new QueueWarningException($"Не найдена очередь с именем {queueName}");
            }

            using (var channel = CreateChannel())
            {
                return channel.MessageCount(queue);
            }
        }

        public void Dispose()
        {
            Dispose(true);

            // подавляем финализацию
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing && _channel != null)
                {
                    // Освобождаем управляемые ресурсы
                    _channel.Close();
                    _channel.Dispose();
                }

                // освобождаем неуправляемые объекты
                _channel = null;
                _logger = null;
                _factory = null;
                _msgInnerProvider = null;
                _queues = null;

                _disposed = true;
            }
        }

        protected abstract string GetExchangeName();

        protected abstract string GetExchangeType();

        protected abstract bool GetQueueDurable();

        protected abstract bool GetQueueAutodelete();

        protected abstract bool GetQueueExclusive();

        /// <summary>
        /// Проверяем что очередь создана.
        /// </summary>
        protected virtual void Init(IEnumerable<string> queues)
        {
            _logger.Debug($"DirectQueueProviderRabbitMQ: create connect to {string.Join(", ", queues)}");

            using (var channel = CreateChannel())
            {
                _logger.Debug($"DirectQueueProviderRabbitMQ: connect open");

                foreach (var item in queues)
                {
                    channel.ExchangeDeclare(GetExchangeName(), GetExchangeType(), true, false, null);

                    var queue = channel.QueueDeclare(item, GetQueueDurable(), GetQueueExclusive(), GetQueueAutodelete(), null);
                    channel.QueueBind(queue, GetExchangeName(), item);
                    _queues.Add(item);
                }
            }
        }

        protected virtual void InitExchange()
        {
            using (var channel = CreateChannel())
            {
                channel.ExchangeDeclare(GetExchangeName(), GetExchangeType(), true, false, null);
            }
        }

        protected virtual string[] GetDistinctQueues(string[] queues)
        {
            queues = queues.CheckNullOrEmpty(nameof(queues)).Distinct().ToArray();

            var queuesToSend = queues
                        .Where(x => _queues.Any(q => q.Equals(x, StringComparison.InvariantCultureIgnoreCase)))
                        .ToArray();

            if (queuesToSend.Count() != queues.Count())
            {
                lock (_locker)
                {
                    var queuesToSend2 = queues
                            .Where(x => _queues.Any(q => q.Equals(x, StringComparison.InvariantCultureIgnoreCase)))
                            .ToArray();

                    if (queuesToSend2.Count() != queues.Count())
                    {
                        var toInitQueues = queues.Except(queuesToSend2).ToArray();
                        Init(toInitQueues);
                        _queues.AddRange(toInitQueues);
                    }
                }
            }

            return queues;
        }

        protected virtual void PushMessageInternal(IEnumerable<string> queues, IQueueMessage message)
        {
            var channel = CreateChannel();
            var body = _msgInnerProvider.PrepareMessageByte(message);

            foreach (var item in queues)
            {
                var bProp = channel.CreateBasicProperties();
                var bHeaders = new Dictionary<string, object> { { "x-delay", message.Delay } };

                bProp.Headers = bHeaders;
                bProp.DeliveryMode = 2;

                channel.BasicPublish(GetExchangeName(), item, bProp, body);
            }
        }

        protected virtual IModel CreateChannel()
        {
            if (_channel == null || !_channel.IsOpen)
            {
                //Лочим на всякий, вдруг один экземпляр провайдера попадет в несколько потоков
                lock (_lockChannel)
                {
                    if (_channel == null || !_channel.IsOpen)
                    {
                        _channel = _factory.CreateConnection().CreateModel();
                    }
                }
            }

            return _channel;
        }
    }
}
