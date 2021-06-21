using bgTeam;
using PartyMaker.Common.Email;
using PartyMaker.Configuration.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PartyMaker.EmailService.Common.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private const int TaskPerClient = 10;
        private const string NoEmailPattern = "<...>";
        private const string HtmlTemplate = "body_html.st4";
        private const string PlainTemplate = "body_text.st4";

        private readonly IMimeMap _mimeMap;
        private readonly ISmtpClientPool _smtpClientPool;
        private readonly IEmailServiceSettings _serviceSettings;
        private readonly ITemplateService _templateService;
        private readonly IAppLogger _logger;

        public EmailSenderService(
            IMimeMap mimeMap,
            ISmtpClientPool smtpClientPool,
            IEmailServiceSettings serviceSettings,
            ITemplateService templateService,
            IAppLogger logger)
        {
            _mimeMap = mimeMap;
            _smtpClientPool = smtpClientPool;
            _serviceSettings = serviceSettings;
            _templateService = templateService;
            _logger = logger;
        }

        public FailedEmailTask Process(EmailTask task)
        {
            return Process(new[] { task }).FirstOrDefault();
        }

        public IEnumerable<FailedEmailTask> Process(IEnumerable<EmailTask> emailTasks)
        {
            var messages = new List<Tuple<EmailTask, MailMessage>>();

            foreach (var task in emailTasks)
            {
                try
                {
                    if (NoEmailPattern.Equals(task.Receiver))
                    {
                        _logger.Info(string.Format("Skip sending email to {0}", task.Receiver));
                        continue;
                    }

                    _logger.Info(string.Format("Building email for {0}", task.Receiver));
                    var builder = new EmailBuilder(
                        _serviceSettings.FromEmail,
                        task.Receiver,
                        task.Subject,
                        _templateService.GetTemplateDir(task.Template));

                    var plainBody = _templateService.CreateFromTemplate(
                        Path.Combine(task.Template, PlainTemplate),
                        task.Substitutions.ToDictionary(x => x.Key, x => x.Value));

                    var htmlBody = _templateService.CreateFromTemplate(
                        Path.Combine(task.Template, HtmlTemplate),
                        task.Substitutions.ToDictionary(x => x.Key, x => x.Value));

                    builder
                        .WithPlainBody(plainBody)
                        .WithHtmlBody(htmlBody);

                    if (task.Attachments != null)
                    {
                        foreach (var attachment in task.Attachments)
                        {
                            var attachmentStream = File.Open(attachment, FileMode.Open);
                            builder.WithAttachment(attachmentStream, Path.GetFileName(attachment),
                                _mimeMap.GetMimeType(Path.GetExtension(attachment)));
                        }
                    }

                    var message = builder.Build();
                    messages.Add(new Tuple<EmailTask, MailMessage>(task, message));
                    _logger.Info(string.Format("Sent email to {0}", task.Receiver));
                }
                catch (FormatException e)
                {
                    _logger.Warning(e.Message);
                }
                catch (Exception e)
                {
                    _logger.Error(e);
                }
            }

            var messageGroups = messages
                .Select((x, i) => new { Index = i, Message = x })
                .GroupBy(x => x.Index / TaskPerClient)
                .Select(x => x.Select(e => e.Message).ToArray())
                .ToArray();

            var tasks = new List<Task<IEnumerable<FailedEmailTask>>>();

            foreach (var itm in messageGroups)
            {
                var @group = itm;
                tasks.Add(Task.Factory.StartNew(() => ProcessMessageGroup(@group)));
            }

            Task.WaitAll(tasks.ToArray());

            return tasks
                .SelectMany(x => x.Result)
                .ToArray();
        }


        private IEnumerable<FailedEmailTask> ProcessMessageGroup(IEnumerable<Tuple<EmailTask, MailMessage>> messageGroup)
        {
            var smtpClient = _smtpClientPool.Get();
            var failedMessages = new List<FailedEmailTask>();

            foreach (var item in messageGroup)
            {
                var task = item.Item1;
                var message = item.Item2;

                try
                {
                    if (!string.IsNullOrEmpty(_serviceSettings.DebugEmail))
                    {
                        var originalReceivers = string.Join("; ", message.To.Select(x => x.Address));
                        message.To.Clear();
                        message.To.Add(_serviceSettings.DebugEmail);
                        message.Subject = string.Format("[{0}] {1}", originalReceivers, message.Subject);
                    }

                    smtpClient.Send(message);
                }
                catch (Exception e)
                {
                    _logger.Error(e);
                    failedMessages.Add(new FailedEmailTask(task, e.Message, e));
                }
                finally
                {
                    message.Dispose();
                }
            }

            _smtpClientPool.Release(smtpClient);

            return failedMessages;
        }
    }
}
