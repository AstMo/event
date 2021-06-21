using bgTeam;
using PartyMaker.Common.ServiceWaiter;
using Polly;
using RabbitMQ.Client;
using System;

namespace PartyMaker.Common.Impl.ServiceWaiter
{
    public class RabbitServiceWaiter : IRabbitServiceWaiter
    {
        private readonly IConnectionFactory _factory;
        private readonly IAppLogger _logger;

        public RabbitServiceWaiter(
            IConnectionFactory factory,
            IAppLogger logger)
        {
            _factory = factory;
            _logger = logger;
        }

        public void Wait()
        {
            Policy
                .Handle<Exception>()
                .WaitAndRetryForever(i => TimeSpan.FromSeconds(2), (result, timeSpan) =>
                {
                    Console.WriteLine($"Create rabbitmq connection failed with {result.Message}. Waiting {timeSpan} before next retry.");
                    _logger.Warning($"Create rabbitmq connection failed with {result.Message}. Waiting {timeSpan} before next retry.");
                })
                .Execute(() =>
                {
                    _factory.CreateConnection().Close();
                });
        }
    }
}
