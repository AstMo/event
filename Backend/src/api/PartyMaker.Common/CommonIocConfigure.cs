using bgTeam;
using bgTeam.Queues;
using PartyMaker.Common.Account;
using PartyMaker.Common.Impl;
using PartyMaker.Common.Impl.Admin;
using PartyMaker.Common.Impl.MessageWaiter;
using PartyMaker.Common.Impl.Queue;
using PartyMaker.Common.Impl.QueueProvider;
using PartyMaker.Common.Impl.ServiceWaiter;
using PartyMaker.Common.MessageWaiter;
using PartyMaker.Common.ServiceWaiter;
using Microsoft.Extensions.DependencyInjection;
using PartyMaker.Common.QueuePrivider;

namespace PartyMaker.Common
{
    public static class CommonIocConfigure
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ISerializer, Serializer>();
            services.AddSingleton<IDeserializer, Deserializer>();

            services.AddSingleton<IDirectQueueProvider, DirectQueueProviderRabbitMQ>();
            services.AddSingleton<ITmpQueueProvider, TmpQueueProviderRabbitMQ>();
            services.AddSingleton<IEmailResponseFanoutQueueProvider, EmailResponseFanoutQueueProviderRabbitMq>();
            services.AddSingleton<IEmailMessageWaiter, EmailMessageWaiter>();

            services.AddSingleton<IRabbitServiceWaiter, RabbitServiceWaiter>();
            services.AddSingleton<IDatabaseServiceWaiter, DatabaseServiceWaiter>();
            services.AddSingleton<IIdentityService, IdentityService>();
            services.AddSingleton<IAccountService, AccountService>();
            services.AddSingleton<IEncodingDetectionService, EncodingDetectionService>();

            services.AddSingleton<IQueueWatcher<IQueueMessage>>(x =>
                new CustomQueueWatcherRabbitMQ(
                    x.GetService<IAppLogger>(),
                    x.GetService<IMessageProvider>(),
                    x.GetService<RabbitMQ.Client.IConnectionFactory>(),
                    1));
        }
    }
}
