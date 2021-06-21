using bgTeam;
using bgTeam.Queues;
using PartyMaker.Common.QueuePrivider;
using PartyMaker.Configuration.Interfaces;
using RabbitMQ.Client;

namespace PartyMaker.Common.Impl.QueueProvider
{
    public class EmailResponseFanoutQueueProviderRabbitMq : BaseResponseFanoutQueueProviderRabbitMq, IEmailResponseFanoutQueueProvider
    {
        private readonly IQueuesSettings _queuesSettings;

        public EmailResponseFanoutQueueProviderRabbitMq(
            IAppLogger logger,
            IMessageProvider msgProvider,
            IConnectionFactory factory,
            IQueuesSettings queuesSettings)
            : base(logger, msgProvider, factory)
        {
            _queuesSettings = queuesSettings;
            InitExchange();
        }

        protected override string GetExchangeName()
        {
            return _queuesSettings.EmailQueue;
        }
    }
}
