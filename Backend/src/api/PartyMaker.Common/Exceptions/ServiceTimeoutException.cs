using System;
using System.Runtime.Serialization;

namespace PartyMaker.Common.Exceptions
{
    [Serializable]
    public class ServiceTimeoutException : Exception
    {
        public ServiceTimeoutException(string message)
            : base(message)
        {
        }

        protected ServiceTimeoutException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
