using System;

namespace PartyMaker.Configuration.Interfaces
{
    public interface IQueuesSettings
    {
        TimeSpan EventResponseTimeout { get; }

        string EmailQueue { get; }

        string DirectExchangeName { get; }
    }
}
