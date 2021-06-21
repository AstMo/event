using System;

namespace PartyMaker.Common.Email
{
    public class FailedEmailTask
    {
        public FailedEmailTask(EmailTask task, string message, Exception exception)
        {
            Exception = exception;
            Message = message;
            Task = task;
        }

        public Exception Exception { get; private set; }

        public string Message { get; private set; }

        public EmailTask Task { get; private set; }
    }
}
