using bgTeam;
using PartyMaker.Common.MessageWaiter;
using PartyMaker.Common.QueuePrivider;
using PartyMaker.Configuration.Interfaces;
using PartyMaker.Dto.Queue;
using System;
using System.Collections.Generic;
using System.Threading;

namespace PartyMaker.Common.Impl.MessageWaiter
{
    public class BaseMessageWaiter : IBaseMessageWaiter, IDisposable
    {
        private readonly IAppLogger _logger;
        private readonly IQueuesSettings _settings;
        private readonly IBaseResponseFanoutQueueProvider _responseFanoutQueueProvider;
        private readonly IDeserializer _deserializer;

        private readonly Dictionary<Guid, ManualResetEvent> _eventResponseMutexes;
        private readonly Dictionary<Guid, ResponseQueueDto> _responses;
        private bool _disposed = false;

        public BaseMessageWaiter(
            IAppLogger logger,
            IQueuesSettings settings,
            IBaseResponseFanoutQueueProvider responseFanoutQueueProvider,
            ITmpQueueProvider tmpQueueProvider,
            IDeserializer deserializer)
        {
            _logger = logger;
            _settings = settings;
            _responseFanoutQueueProvider = responseFanoutQueueProvider;
            _deserializer = deserializer;
            _eventResponseMutexes = new Dictionary<Guid, ManualResetEvent>();
            _responses = new Dictionary<Guid, ResponseQueueDto>();
            tmpQueueProvider.Received += ProcessSuzResponse;
            tmpQueueProvider.StartListen(_responseFanoutQueueProvider.ExchangeName);
        }

        ~BaseMessageWaiter()
        {
            Dispose(false);
        }

        public ResponseQueueDto WaitForResponse(Guid requestId)
        {
            var resetEvent = StoreResponseWaitingEvent(requestId);
            return WaitForResponse(resetEvent, requestId);
        }

        public void Dispose()
        {
            Dispose(true);

            // подавляем финализацию
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _responseFanoutQueueProvider?.Dispose();
                }

                _disposed = true;
            }
        }

        private void ProcessSuzResponse(string message)
        {
            var responseDto = _deserializer.Deserialize<ResponseQueueDto>(message);
            _logger.Info($"Got acknowledgement for {responseDto.RequestId}");
            ProcessRequestAck(responseDto);
        }

        private ResponseQueueDto WaitForResponse(ManualResetEvent resetEvent, Guid requestId)
        {
            var waitResponse = resetEvent?.WaitOne(_settings.EventResponseTimeout);
            if (waitResponse.HasValue && waitResponse.Value)
            {
                return _responses[requestId];
            }

            return new ResponseQueueDto() { IsTimedOut = true };
        }

        private ManualResetEvent StoreResponseWaitingEvent(Guid requestId)
        {
            var manualResetEvent = new ManualResetEvent(false);
            _eventResponseMutexes.Add(requestId, manualResetEvent);
            return manualResetEvent;
        }

        private void ProcessRequestAck(ResponseQueueDto responseDto)
        {
            if (_eventResponseMutexes.ContainsKey(responseDto.RequestId))
            {
                _responses.Add(responseDto.RequestId, responseDto);
                var mutex = _eventResponseMutexes[responseDto.RequestId];
                mutex.Set();
                _eventResponseMutexes.Remove(responseDto.RequestId);
            }
        }
    }
}
