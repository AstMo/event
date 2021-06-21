using bgTeam;
using PartyMaker.Common.Email;
using PartyMaker.EmailService.Common.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PartyMaker.Story.Email
{
    class EmailServiceRestorePasswordStory : IStory<EmailServiceRestorePasswordStoryContext, bool>
    {
        private readonly IAppLogger _logger;
        private readonly IEmailSenderService _emailSenderService;

        public EmailServiceRestorePasswordStory (
            IAppLogger logger,
            IEmailSenderService emailSenderService)
        {
            _logger = logger;
            _emailSenderService = emailSenderService;
        }

        public bool Execute(EmailServiceRestorePasswordStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<bool> ExecuteAsync(EmailServiceRestorePasswordStoryContext context)
        {
            return await Task.Factory.StartNew(() =>
            {
                _logger.Info("Рассылка уведомления о изменении пароля ");

                var task = new EmailTask
                {
                    Subject = "Новый запрос на обратный вызов",
                    Receiver = context.Receiver,
                    Template = "service/Restore",
                    Substitutions = new List<EmailSubstitution>()
                        {
                            new EmailSubstitution()
                            {
                                Key = "Password",
                                Value = context.Password,
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