using bgTeam.Queues;

namespace PartyMaker.Configuration.Models
{
    public class QueueSettings : IQueueProviderSettings
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public string VirtualHost { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }
    }
}
