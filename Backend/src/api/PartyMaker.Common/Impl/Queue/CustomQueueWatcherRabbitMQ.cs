﻿using bgTeam;
using bgTeam.Queues;
using bgTeam.Queues.Exceptions;
using MongoDB.Bson;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PartyMaker.Common.Impl.Queue
{
    public class CustomQueueWatcherRabbitMQ : IQueueWatcher<IQueueMessage>
    {
        private readonly IAppLogger _logger;
        private readonly IConnectionFactory _factory;
        private readonly IMessageProvider _msgProvider;

        private readonly int _threadSleepMs;
        private readonly SemaphoreSlim _semaphore;

        public CustomQueueWatcherRabbitMQ(
            IAppLogger logger,
            IMessageProvider msgProvider,
            IQueueProviderSettings settings,
            int threadsCount = 1)
        {
            _logger = logger;
            _msgProvider = msgProvider;
            _factory = new ConnectionFactory()
            {
                HostName = settings.Host,
                Port = settings.Port,
                VirtualHost = settings.VirtualHost,
                UserName = settings.Login,
                Password = settings.Password,
            };

            _semaphore = new SemaphoreSlim(threadsCount, threadsCount);

            _threadSleepMs = 10;
        }

        public CustomQueueWatcherRabbitMQ(
            IAppLogger logger,
            IMessageProvider msgProvider,
            IConnectionFactory factory,
            int threadsCount = 1)
        {
            _logger = logger;
            _msgProvider = msgProvider;
            _factory = factory;

            _semaphore = new SemaphoreSlim(threadsCount, threadsCount);

            _threadSleepMs = 10;
        }

        public event QueueMessageHandler Subscribe;

        public event EventHandler<ExtThreadExceptionEventArgs> Error;

        public void StartWatch(string queueName)
        {
            if (Subscribe == null)
            {
                throw new ArgumentNullException("queueName");
            }

            while (true)
            {
                // _logger.Debug($"NewQueueWatcherRabbitMQ: create connection");
                using (var connection = _factory.CreateConnection())
                {
                    try
                    {
                        MainLoop(queueName, connection);
                    }
                    catch (Exception ex)
                    {
                        var args = connection.CloseReason;
                        if (args != null)
                        {
                            _logger.Error($"NewQueueWatcherRabbitMQ: connection shutdown. Reason: {args.ReplyCode} - {args.ReplyText}");
                        }
                        else
                        {
                            _logger.Error($"NewQueueWatcherRabbitMQ: error: {ex.Message}");
                        }
                    }
                }
            }
        }

        protected async Task<bool> AskMessage(string queueName, IConnection connection)
        {
            using (var channel = connection.CreateModel())
            {
                var res = channel.BasicGet(queueName, false);
                if (res != null)
                {
                    var message = _msgProvider.ExtractObject(Encoding.UTF8.GetString(res.Body.ToArray()));
                    Exception exp = null;
                    try
                    {
                        await Subscribe(message);
                    }
                    catch (Exception ex)
                    {
                        exp = ex;
                    }
                    finally
                    {
                        channel.BasicAck(res.DeliveryTag, false);
                    }

                    if (exp != null)
                    {
                        throw new QueueWatcherException($"bgTeam: {exp.Message}", message, exp);
                    }

                    return true;
                }
            }

            return false;
        }

        private void MainLoop(string queueName, IConnection connection)
        {
            while (true)
            {
                _semaphore.Wait();
                Task.Factory.StartNew(async () =>
                {
                    var noMsg = false;

                    try
                    {
                        noMsg = await AskMessage(queueName, connection);

                        if (!noMsg)
                        {
                            await Task.Delay(_threadSleepMs);
                        }
                    }
                    catch (QueueWatcherException exp) when (exp.InnerException is QueueWatcherWarningException qexp)
                    {
                        _logger.Warning($"Exception of type {qexp.GetType().Name}: {qexp.Message}{Environment.NewLine}{qexp.StackTrace}");

                        Error?.Invoke(this, new ExtThreadExceptionEventArgs(exp.QueueMessage, qexp));
                    }
                    catch (QueueWatcherException exp)
                    {
                        _logger.Error(exp);

                        Error?.Invoke(this, new ExtThreadExceptionEventArgs(exp.QueueMessage, exp.GetBaseException()));
                    }
                    catch (Exception exp)
                    {
                        _logger.Fatal(exp);

                        // Ждём 5 сек
                        await Task.Delay(5000);
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                });
            }
        }
    }
}
