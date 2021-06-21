using bgTeam.Core;
using bgTeam.Queues;
using PartyMaker.Common;
using PartyMaker.Configuration.Interfaces;
using PartyMaker.Configuration.Models;
using Microsoft.Extensions.Configuration;

namespace PartyMaker.Application
{
    internal class AppSettings : IAppSettings, IAppLoggerConfig
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettings"/> class.
        /// </summary>
        /// <param name="config">app config.</param>
        public AppSettings(Microsoft.Extensions.Configuration.IConfiguration config, ISerializer serializer)
        {

            _config = config;

            AuthenticationSettings = new AuthenticationSettings();
            config.GetSection("Authentication").Bind(AuthenticationSettings);

            DatabaseSettings = new DatabaseSettings();
            config.GetSection("Database").Bind(DatabaseSettings);

            QueuesSettings = new QueuesSettings();
            config.GetSection("Queues").Bind(QueuesSettings);

            QueueProviderSettings = new QueueProviderSettings();
            config.GetSection("Queue").Bind(QueueProviderSettings);
            System.Console.WriteLine(QueueProviderSettings.Host);
            System.Console.WriteLine(QueueProviderSettings.Port);

            ImageStoreSettings = new ImageStoreSettings();
            config.GetSection("ImageSetting").Bind(ImageStoreSettings);

            WebAppConfigSettings = new WebAppConfigSettings();
            config.GetSection("WebAppSettings").Bind(WebAppConfigSettings);
        }

        /// <summary>
        /// Íàñòðîéêè äîñòóïà ê ÁÄ.
        /// </summary>

        public IAuthenticationSettings AuthenticationSettings { get; }

        public IDatabaseSettings DatabaseSettings { get; }

        public IQueuesSettings QueuesSettings { get; }

        public IImageStoreSetting ImageStoreSettings { get; }

        public IQueueProviderSettings QueueProviderSettings { get; }

        public IWebAppConfigSettings WebAppConfigSettings { get; }

        public string LoggerName => "WebAppLog";

        public IConfigurationSection GetLoggerConfig()
        {
            return _config.GetSection("Serilog");
        }
    }
}
