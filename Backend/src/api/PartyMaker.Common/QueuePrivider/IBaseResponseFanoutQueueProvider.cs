using bgTeam.Queues;

namespace PartyMaker.Common.QueuePrivider
{
    public interface IBaseResponseFanoutQueueProvider : IQueueProvider
    {
        string ExchangeName { get; }

        string ExchangeType { get; }
    }
}
