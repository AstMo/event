using PartyMaker.Common.Email;
using System.Collections.Generic;

namespace PartyMaker.EmailService.Common.Services
{
    public interface IEmailSenderService
    {
        FailedEmailTask Process(EmailTask task);

        IEnumerable<FailedEmailTask> Process(IEnumerable<EmailTask> emailTasks);
    }
}
