using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace PartyMaker.Common.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class ServiceDiscoveryException : Exception
    {
        public ServiceDiscoveryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ServiceDiscoveryException(string message)
            : base(message)
        {
        }

        protected ServiceDiscoveryException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
