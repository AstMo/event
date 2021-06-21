using bgTeam.Queues;
using PartyMaker.Configuration.Interfaces;

namespace PartyMaker.EmailService.Host
{
    public interface IAppSettings
    {
        IQueueProviderSettings QueueProviderSettings { get; }

        IQueuesSettings QueuesSettings { get; }

        IEmailServiceSettings EmailSettings { get; }
    }
}
