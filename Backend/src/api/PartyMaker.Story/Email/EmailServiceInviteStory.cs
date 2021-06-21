using bgTeam;
using PartyMaker.Common.Email;
using PartyMaker.EmailService.Common.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PartyMaker.Story.Email
{
    public class EmailServiceInviteStory : IStory<EmailServiceInviteStoryContext, bool>
    {
        private readonly IAppLogger _logger;
        private readonly IEmailSenderService _emailSenderService;

        public EmailServiceInviteStory(
            IAppLogger logger,
            IEmailSenderService emailSenderService)
        {
            _logger = logger;
            _emailSenderService = emailSenderService;
        }

        public bool Execute(EmailServiceInviteStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<bool> ExecuteAsync(EmailServiceInviteStoryContext context)
        {
            return await Task.Factory.StartNew(() =>
            {
                _logger.Info("Рассылка уведомления о приглашении на мероприятие");

                var task = new EmailTask
                {
                    Subject = "Новый запрос на обратный вызов",
                    Receiver = context.Reciever,
                    Template = "service/Registration",
                    Substitutions = new List<EmailSubstitution>()
                        {
                            new EmailSubstitution()
                            {
                                Key = "Link",
                                Value = context.Link,
                            },
                            new EmailSubstitution()
                            {
                                Key = "EventName",
                                Value = context.EventName,
                            },
                        },
                };

                var result = _emailSenderService.Process(task);

                if (result != null)
                {
                    _logger.Error(string.Format("Не удалось отправить письма следующим адресатам: {0}, {1}", result.Task.Receiver, result.Exception.Message));
                    throw result.Exception;
                }

                return true;
            });
        }
    }
}
