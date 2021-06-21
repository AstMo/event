using PartyMaker.Common.Impl;
using PartyMaker.Configuration.Interfaces;
using System.Net;
using System.Net.Mail;

namespace PartyMaker.EmailService.Common.Services
{
    public class SmtpClientPool : ObjectPool<SmtpClient>, ISmtpClientPool
    {
        private readonly IEmailServiceSettings _serviceSettings;

        public SmtpClientPool(IEmailServiceSettings serviceSettings)
            : base(serviceSettings.SmtpPoolMaxSize)
        {
            _serviceSettings = serviceSettings;

            SetGenerator(Generator);
            Prepopulate(serviceSettings.SmtpPoolInitialSize);
        }

        private SmtpClient Generator()
        {
            var smtpClient = new SmtpClient(_serviceSettings.SmtpHost, _serviceSettings.SmtpPort)
            {
                Credentials = new NetworkCredential(_serviceSettings.SmtpUsername, _serviceSettings.SmtpPassword),
                EnableSsl = _serviceSettings.SmtpUseSsl
            };

            return smtpClient;
        }
    }
}
