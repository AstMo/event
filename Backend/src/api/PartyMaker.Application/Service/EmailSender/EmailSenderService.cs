using bgTeam.DataAccess;
using bgTeam.Impl.Rabbit;
using PartyMaker.Common;
using PartyMaker.Common.Email;
using PartyMaker.Common.MessageWaiter;
using PartyMaker.Configuration.Interfaces;
using PartyMaker.Domain.Entities;
using PartyMaker.Dto.Queue;
using PartyMaker.Dto.WebApp;
using System;
using System.Threading.Tasks;

namespace PartyMaker.Application.Service.EmailSender
{
    public class EmailSenderService : IWebAppEmailSenderService
    {
        private readonly IRepository _repository;
        private readonly ISerializer _serializer;
        private readonly IDeserializer _deserializer;
        private readonly IDirectQueueProvider _queueProvider;
        private readonly IEmailMessageWaiter _messageWaiter;
        private readonly IQueuesSettings _queuesSettings;
        private readonly IWebAppConfigSettings _webAppConfigSettings;

        public EmailSenderService(
            IRepository repository,
            ISerializer serializer,
            IDeserializer deserializer,
            IDirectQueueProvider queueProvider,
            IEmailMessageWaiter messageWaiter,
            IQueuesSettings queuesSettings,
            IWebAppConfigSettings webAppConfigSettings)
        {
            _repository = repository;
            _serializer = serializer;
            _deserializer = deserializer;
            _queueProvider = queueProvider;
            _messageWaiter = messageWaiter;
            _queuesSettings = queuesSettings;
            _webAppConfigSettings = webAppConfigSettings;
        }

        public bool SendInviteUserInfo(string email, string eventName)
        {
            var requestId = Guid.NewGuid();
            var request = new EmailRequestQueueDto
            {
                RequestId = requestId,
                EmailRequestType = EEmailRequestType.Invite,
                Params = _serializer.Serialize(new QueueInviteRequestDto
                {
                    Email = email,
                    EventName = eventName,
                    Link = string.Format(_webAppConfigSettings.InviteLinkFormat,
                        Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(email)).Replace('-', '+').Replace('/', '_'))
                })
            };
            var objectToRabbit = _serializer.Serialize(request);
            _queueProvider.PushMessage(new QueueMessageRabbitMQ { Body = objectToRabbit, Uid = requestId }, _queuesSettings.EmailQueue);

            var rabbitResponse = _messageWaiter.WaitForResponse(requestId);
            return rabbitResponse.IsSuccess;
        }

        public async Task<bool> SendRestoreInfo(string email, string password)
        {
            var user = await _repository.GetAsync<User>(t => t.Email == email);
            if (user == null)
                return false;

            var requestId = Guid.NewGuid();
            var request = new EmailRequestQueueDto
            {
                RequestId = requestId,
                EmailRequestType = EEmailRequestType.RestorePassword,
                Params = _serializer.Serialize(new QueueRestoreRequestDto
                {
                    Email = user.Email,
                    NewPassword = password
                })
            };
            var objectToRabbit = _serializer.Serialize(request);
            _queueProvider.PushMessage(new QueueMessageRabbitMQ { Body = objectToRabbit, Uid = requestId }, _queuesSettings.EmailQueue);

            var rabbitResponse = _messageWaiter.WaitForResponse(requestId);
            if (rabbitResponse.IsSuccess)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> SendRegistrationInfo(string email)
        {
            var user = await _repository.GetAsync<User>(t => t.Email == email);
            if (user == null)
                return false;

            var requestId = Guid.NewGuid();
            var request = new EmailRequestQueueDto
            {
                RequestId = requestId,
                EmailRequestType = EEmailRequestType.Registration,
                Params = _serializer.Serialize(new QueueRegistrationRequestDto
                {
                    Email = user.Email,
                    Link = string.Format(_webAppConfigSettings.LinkFormat, user.LinkHash)
                })
            };
            var objectToRabbit = _serializer.Serialize(request);
            _queueProvider.PushMessage(new QueueMessageRabbitMQ { Body = objectToRabbit, Uid = requestId }, _queuesSettings.EmailQueue);

            var rabbitResponse = _messageWaiter.WaitForResponse(requestId);
            if (rabbitResponse.IsSuccess)
            {
                return true;
            }

            return false;
        }
    }
}
