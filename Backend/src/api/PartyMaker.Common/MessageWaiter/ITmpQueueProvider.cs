using System;

namespace PartyMaker.Common.MessageWaiter
{
    public interface ITmpQueueProvider
    {
        event Action<string> Received;

        void StartListen(string exchangeName);
    }
}
