using PartyMaker.Configuration.Interfaces;
using System;

namespace PartyMaker.Configuration.Models
{
    public class EmailServiceSettings : IEmailServiceSettings
    {
        public int SmtpPoolMaxSize { get; set; }

        public int SmtpPoolInitialSize { get; set; }

        public string SmtpUsername { get; set; }

        public string SmtpHost { get; set; }

        public int SmtpPort { get; set; }

        public string SmtpPassword { get; set; }

        public bool SmtpUseSsl { get; set; }

        public string FromEmail { get; set; }

        public string DebugEmail { get; set; }

        public TimeSpan IncomingMessageCheckInterval { get; set; }

        public TimeSpan HandlingMessageTimeout { get; set; }

        public string Recipient { get; set; }
    }
}
