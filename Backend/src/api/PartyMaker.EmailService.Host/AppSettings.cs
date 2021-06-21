using bgTeam.Core;
using bgTeam.Queues;
using Microsoft.Extensions.Configuration;
using PartyMaker.Configuration.Interfaces;
using PartyMaker.Configuration.Models;

namespace PartyMaker.EmailService.Host
{
    internal class AppSettings : IAppSettings, IAppLoggerConfig
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration _config;

        public AppSettings(Microsoft.Extensions.Configuration.IConfiguration config)
        {
            _config = config;
            QueueProviderSettings = new QueueSettings();
            config.GetSection("Queue").Bind(QueueProviderSettings);

            QueuesSettings = new QueuesSettings();
            config.GetSection("Queues").Bind(QueuesSettings);

            EmailSettings = new EmailServiceSettings();
            config.GetSection("EmailService").Bind(EmailSettings);

        }

        public string LoggerName => "SuzAppLog";

        public IQueueProviderSettings QueueProviderSettings { get; }

        public IQueuesSettings QueuesSettings { get; }

        public IEmailServiceSettings EmailSettings { get; }

        public IConfigurationSection GetLoggerConfig()
        {
            return _config.GetSection("Serilog");
        }
    }
}
