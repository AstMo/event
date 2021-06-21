using System;
using System.Runtime.Serialization;

namespace PartyMaker.Common.Exceptions
{
    [Serializable]
    public class ObjectNotFoundException : Exception
    {
        public ObjectNotFoundException(string message)
            : base(message)
        {
        }

        protected ObjectNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
