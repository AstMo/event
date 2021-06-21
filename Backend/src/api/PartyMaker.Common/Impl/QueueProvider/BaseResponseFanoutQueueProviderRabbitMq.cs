using bgTeam;
using bgTeam.Queues;
using PartyMaker.Common.QueuePrivider;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;

namespace PartyMaker.Common.Impl.QueueProvider
{
    public abstract class BaseResponseFanoutQueueProviderRabbitMq : BaseQueueProviderRabbitMQ, IBaseResponseFanoutQueueProvider
    {
        protected BaseResponseFanoutQueueProviderRabbitMq(
            IAppLogger logger,
            IMessageProvider msgProvider,
            IConnectionFactory factory)
            : base(logger, msgProvider, factory)
        {
        }

        public string ExchangeName => GetExchangeName();

        public string ExchangeType => GetExchangeType();

        public override void PushMessage(IQueueMessage message)
        {
            var channel = CreateChannel();
            var body = MsgInnerProvider.PrepareMessageByte(message);

            var bProp = channel.CreateBasicProperties();
            var bHeaders = new Dictionary<string, object> { { "x-delay", message.Delay } };

            bProp.Headers = bHeaders;
            bProp.DeliveryMode = 2;

            channel.BasicPublish(GetExchangeName(), string.Empty, bProp, body);
        }

        public override void PushMessage(IQueueMessage message, params string[] queues)
        {
            throw new NotImplementedException();
        }

        protected override string GetExchangeType()
        {
            return "fanout";
        }

        protected override bool GetQueueDurable()
        {
            return true;
        }

        protected override bool GetQueueAutodelete()
        {
            return true;
        }

        protected override bool GetQueueExclusive()
        {
            return true;
        }
    }
}
