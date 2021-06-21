using System;

namespace PartyMaker.Dto.Queue
{
    public class ResponseQueueDto
    {
        public ResponseQueueDto()
        {
            RequestId = Guid.Empty;
            IsSuccess = false;
            Message = string.Empty;
            IsTimedOut = false;
            Data = string.Empty;
        }

        private ResponseQueueDto(Guid requestId, bool isSuccess)
        {
            RequestId = requestId;
            IsSuccess = isSuccess;
            Message = string.Empty;
            IsTimedOut = false;
            Data = string.Empty;
        }

        private ResponseQueueDto(Guid requestId, bool isSuccess, string message, bool isTimedOut)
        {
            RequestId = requestId;
            IsSuccess = isSuccess;
            Message = message;
            IsTimedOut = isTimedOut;
            Data = string.Empty;
        }

        public Guid RequestId { get; set; }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public bool IsTimedOut { get; set; }

        public string Data { get; set; }

        public static ResponseQueueDto Success(Guid messageId)
        {
            return new ResponseQueueDto(messageId, true);
        }

        public static ResponseQueueDto Failed(Guid messageId)
        {
            return new ResponseQueueDto(messageId, false);
        }

        public static ResponseQueueDto Failed(Guid messageId, string message)
        {
            return new ResponseQueueDto(messageId, false, message, false);
        }
    }
}
