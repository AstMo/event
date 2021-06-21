using System.Threading.Tasks;

namespace PartyMaker.Common.Email
{
    public interface IWebAppEmailSenderService
    {
        Task<bool> SendRegistrationInfo(string email);

        bool SendInviteUserInfo(string email, string eventName);

        Task<bool> SendRestoreInfo(string email, string password);
    }
}
