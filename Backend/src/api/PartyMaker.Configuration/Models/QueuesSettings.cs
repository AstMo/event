using PartyMaker.Configuration.Interfaces;
using System;

namespace PartyMaker.Configuration.Models
{
    public class QueuesSettings : IQueuesSettings
    {
        public TimeSpan EventResponseTimeout { get; set; }

        public string EmailQueue { get; set; }

        public string DirectExchangeName { get; set; }
    }
}
