using System;
using System.Net;
using System.Runtime.Serialization;

namespace PartyMaker.Common.Exceptions
{
    [Serializable]
    public class HttpCommunicationException : Exception
    {
        public HttpCommunicationException(HttpStatusCode responseStatusCode, string message)
            : base(message)
        {
            ResponseStatusCode = responseStatusCode;
        }

        public HttpCommunicationException(Exception exception)
            : base("Exception occured", exception)
        {
        }

        protected HttpCommunicationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }

        public HttpStatusCode ResponseStatusCode { get; }
    }
}
