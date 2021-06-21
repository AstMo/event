using PartyMaker.Dto.Queue;
using System;

namespace PartyMaker.Common.MessageWaiter
{
    public interface IBaseMessageWaiter
    {
        ResponseQueueDto WaitForResponse(Guid requestId);
    }
}
