using bgTeam;
using bgTeam.DataAccess;
using PartyMaker.Common.ServiceWaiter;
using Polly;
using System;

namespace PartyMaker.Common.Impl.ServiceWaiter
{
    public class DatabaseServiceWaiter : IDatabaseServiceWaiter
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IAppLogger _logger;

        public DatabaseServiceWaiter(
            IConnectionFactory connectionFactory,
            IAppLogger logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public void Wait()
        {
            Policy
                .Handle<Exception>()
                .WaitAndRetryForever(
                    i => TimeSpan.FromSeconds(2),
                    (result, timeSpan) =>
                    {
                        _logger.Warning(
                            $"Create database connection failed with {result.Message}. Waiting {timeSpan} before next retry.");
                    })
                .Execute(() =>
                {
                    var connection = _connectionFactory.Create();
                    connection.Dispose();
                });
        }
    }
}
