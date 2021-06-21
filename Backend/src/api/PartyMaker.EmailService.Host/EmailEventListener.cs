using bgTeam;
using bgTeam.Impl.Rabbit;
using bgTeam.Queues;
using bgTeam.Queues.Exceptions;
using Newtonsoft.Json;
using PartyMaker.Common;
using PartyMaker.Common.QueuePrivider;
using PartyMaker.Configuration.Interfaces;
using PartyMaker.Dto.Queue;
using PartyMaker.Story.Email;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace PartyMaker.EmailServiceHost
{
    public class EmailEventListener
    {
        private readonly IAppLogger _logger;
        private readonly IQueueWatcher<IQueueMessage> _queueWatcher;
        private readonly IStoryBuilder _storyBuilder;
        private readonly IEmailServiceSettings _emailServiceSettings;
        private readonly IQueuesSettings _queuesSettings;
        private readonly IDeserializer _deserializer;
        private readonly IEmailResponseFanoutQueueProvider _suzResponseFanoutQueueProvider;
        private readonly IDirectQueueProvider _directQueueProvider;
        private readonly ISerializer _serializer;

        public EmailEventListener(
            IAppLogger logger,
            IQueueWatcher<IQueueMessage> queueWatcher,
            IStoryBuilder storyBuilder,
            IEmailServiceSettings emailServiceSettings,
            IQueuesSettings queuesSettings,
            IDeserializer deserializer,
            IEmailResponseFanoutQueueProvider suzResponseFanoutQueueProvider,
            IDirectQueueProvider directQueueProvider,
            ISerializer serializer)
        {
            _logger = logger;
            _queueWatcher = queueWatcher;
            _storyBuilder = storyBuilder;
            _emailServiceSettings = emailServiceSettings;
            _queuesSettings = queuesSettings;
            _deserializer = deserializer;
            _suzResponseFanoutQueueProvider = suzResponseFanoutQueueProvider;
            _directQueueProvider = directQueueProvider;
            _serializer = serializer;
        }

        public void StartListen()
        {
            _queueWatcher.Subscribe += message =>
            {
                return Task.Run(() =>  ProcessSendEmail(message));
            };
            _queueWatcher.Error += ProcessError;
            Task.Run(() => _queueWatcher.StartWatch(_queuesSettings.EmailQueue));
            _logger.Info("Start listening");
        }

        public void StopListen()
        {
            _logger.Info("Stop listening");
        }

        private void ProcessError(object sender, ExtThreadExceptionEventArgs e)
        {
            _logger.Error(e.Exception);
        }

        private async Task ProcessSendEmail(IQueueMessage message)
        {
            var request = _deserializer.Deserialize<EmailRequestQueueDto>(message.Body);
            try
            {
                await RunStoryForRequest(request);
                _suzResponseFanoutQueueProvider.PushMessage(new QueueMessageRabbitMQ(_serializer.Serialize(ResponseQueueDto.Success(request.RequestId))));
            }
            catch (Exception ex)
            {
                _suzResponseFanoutQueueProvider.PushMessage(new QueueMessageRabbitMQ(_serializer.Serialize(ResponseQueueDto.Failed(request.RequestId, ex.Message))));
            }
        }

        private async Task RunStoryForRequest(EmailRequestQueueDto request)
        {
            _logger.Info(JsonConvert.SerializeObject(request));

            switch (request.EmailRequestType)
            {
                case EEmailRequestType.Registration:
                    var emailServiceQuestionDto = _deserializer.Deserialize<QueueRegistrationRequestDto>(request.Params);
                    await _storyBuilder
                        .Build(new EmailServiceRegistrationStoryContext
                        {
                            Receiver = emailServiceQuestionDto.Email,
                            Link = emailServiceQuestionDto.Link,
                        })
                        .ReturnAsync<bool>();
                    break;
                case EEmailRequestType.Invite:
                    var emailServiceBackcallDto = _deserializer.Deserialize<QueueInviteRequestDto>(request.Params);
                    await _storyBuilder
                        .Build(new EmailServiceInviteStoryContext
                        {
                            Link = emailServiceBackcallDto.Link,
                            EventName = emailServiceBackcallDto.EventName,
                            Reciever = emailServiceBackcallDto.Email
                        })
                        .ReturnAsync<bool>();
                    break;
                case EEmailRequestType.RestorePassword:
                    var emailServiceRestorePaswordDto = _deserializer.Deserialize<QueueRestoreRequestDto>(request.Params);
                    await _storyBuilder
                        .Build(new EmailServiceRestorePasswordStoryContext
                        {
                            Password = emailServiceRestorePaswordDto.NewPassword,
                            Receiver = emailServiceRestorePaswordDto.Email
                        }).ReturnAsync<bool>();
                    break;
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}
