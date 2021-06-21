using System;
using System.Runtime.Serialization;

namespace PartyMaker.Common.Exceptions
{
    [Serializable]
    public class ObjectNotInitializedException : Exception
    {
        public ObjectNotInitializedException(string message)
            : base(message)
        {
        }

        protected ObjectNotInitializedException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
