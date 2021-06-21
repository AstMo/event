using bgTeam.Queues;
using MongoDB.Bson;
using PartyMaker.Common.MessageWaiter;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace PartyMaker.Common.Impl.MessageWaiter
{
    public class TmpQueueProviderRabbitMQ : ITmpQueueProvider, IDisposable
    {
        private readonly IConnectionFactory _factory;
        private readonly IMessageProvider _msgProvider;
        private bool disposed = false;
        private IConnection _connection;
        private IModel _channel;

        public TmpQueueProviderRabbitMQ(
            IConnectionFactory factory,
            IMessageProvider msgProvider)
        {
            _factory = factory;
            _msgProvider = msgProvider;
        }

        ~TmpQueueProviderRabbitMQ()
        {
            Dispose(false);
        }

        public event Action<string> Received;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void StartListen(string exchangeName)
        {
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(
                queue: queueName,
                exchange: exchangeName,
                routingKey: string.Empty);
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, args) => ProcessSuzResponse(_msgProvider.ExtractObject(Encoding.UTF8.GetString(args.Body.ToArray())));
            _channel.BasicConsume(queueName, true, consumer);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                _channel?.Dispose();
                _connection?.Dispose();
            }

            disposed = true;
        }

        private void ProcessSuzResponse(IQueueMessage message)
        {
            Received?.Invoke(message.Body);
        }
    }
}
