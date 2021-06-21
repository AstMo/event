using bgTeam.Queues;
using PartyMaker.Configuration.Interfaces;

namespace PartyMaker.Application
{
    internal interface IAppSettings
    {
        IAuthenticationSettings AuthenticationSettings { get; }

        IDatabaseSettings DatabaseSettings { get; }

        IQueuesSettings QueuesSettings { get; }

        IQueueProviderSettings QueueProviderSettings { get; }

        IImageStoreSetting ImageStoreSettings { get; }

        IWebAppConfigSettings WebAppConfigSettings { get; }
    }
}
