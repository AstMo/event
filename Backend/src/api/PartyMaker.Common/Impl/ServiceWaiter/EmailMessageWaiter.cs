using bgTeam;
using PartyMaker.Common.Impl.MessageWaiter;
using PartyMaker.Common.MessageWaiter;
using PartyMaker.Common.QueuePrivider;
using PartyMaker.Configuration.Interfaces;

namespace PartyMaker.Common.Impl.ServiceWaiter
{
    public class EmailMessageWaiter : BaseMessageWaiter, IEmailMessageWaiter
    {
        public EmailMessageWaiter(
            IAppLogger logger,
            IQueuesSettings settings,
            IEmailResponseFanoutQueueProvider responseFanoutQueueProvider,
            ITmpQueueProvider tmpQueueProvider,
            IDeserializer deserializer)
            : base(logger, settings, responseFanoutQueueProvider, tmpQueueProvider, deserializer)
        {
        }
    }
}
