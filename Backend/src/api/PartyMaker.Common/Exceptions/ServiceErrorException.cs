using System;
using System.Runtime.Serialization;

namespace PartyMaker.Common.Exceptions
{
    [Serializable]
    public class ServiceErrorException : Exception
    {
        public ServiceErrorException(string message)
            : base(message)
        {
        }

        protected ServiceErrorException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
