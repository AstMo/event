using System;

namespace PartyMaker.Configuration.Interfaces
{
    public interface IEmailServiceSettings
    {
        int SmtpPoolMaxSize { get; }

        int SmtpPoolInitialSize { get; }

        string SmtpUsername { get; }

        string SmtpHost { get; }

        int SmtpPort { get; }

        string SmtpPassword { get; }

        bool SmtpUseSsl { get; }

        string FromEmail { get; }

        string Recipient { get; }

        string DebugEmail { get; }

        TimeSpan IncomingMessageCheckInterval { get; }

        TimeSpan HandlingMessageTimeout { get; }
    }
}
