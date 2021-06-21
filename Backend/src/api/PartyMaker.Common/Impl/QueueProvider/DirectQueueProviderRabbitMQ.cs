using bgTeam;
using bgTeam.Queues;
using PartyMaker.Configuration.Interfaces;
using RabbitMQ.Client;

namespace PartyMaker.Common.Impl.QueueProvider
{
    public sealed class DirectQueueProviderRabbitMQ : BaseQueueProviderRabbitMQ, IDirectQueueProvider
    {
        private readonly IQueuesSettings _queuesSettings;

        public DirectQueueProviderRabbitMQ(
            IAppLogger logger,
            IQueuesSettings queuesSettings,
            IMessageProvider msgProvider,
            IConnectionFactory factory)
            : base(logger, msgProvider, factory)
        {
            _queuesSettings = queuesSettings;
            Init(new[]
            {
                queuesSettings.EmailQueue,
            });
        }

        protected override string GetExchangeName()
        {
            return _queuesSettings.DirectExchangeName;
        }

        protected override string GetExchangeType()
        {
            return "direct";
        }

        protected override bool GetQueueDurable()
        {
            return true;
        }

        protected override bool GetQueueAutodelete()
        {
            return false;
        }

        protected override bool GetQueueExclusive()
        {
            return false;
        }
    }
}
