using bgTeam;
using PartyMaker.Common.Email;
using PartyMaker.EmailService.Common.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PartyMaker.Story.Email
{
    class EmailServiceRegistrationStory : IStory<EmailServiceRegistrationStoryContext, bool>
    {
        private readonly IAppLogger _logger;
        private readonly IEmailSenderService _emailSenderService;

        public EmailServiceRegistrationStory(
            IAppLogger logger,
            IEmailSenderService emailSenderService)
        {
            _logger = logger;
            _emailSenderService = emailSenderService;
        }

        public bool Execute(EmailServiceRegistrationStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<bool> ExecuteAsync(EmailServiceRegistrationStoryContext context)
        {
            return await Task.Factory.StartNew(() =>
            {
                _logger.Info("Рассылка уведомления о подтверждении регистрации");

                var task = new EmailTask
                {
                    Subject = "Новый запрос на обратный вызов",
                    Receiver = context.Receiver,
                    Template = "service/Registration",
                    Substitutions = new List<EmailSubstitution>()
                        {
                            new EmailSubstitution()
                            {
                                Key = "Link",
                                Value = context.Link,
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