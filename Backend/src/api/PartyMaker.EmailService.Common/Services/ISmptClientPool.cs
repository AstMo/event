using System.Net.Mail;

namespace PartyMaker.EmailService.Common.Services
{
    public interface ISmtpClientPool
    {
        SmtpClient Get();
        void Release(SmtpClient client);
    }
}
