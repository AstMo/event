using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;

namespace PartyMaker.Common.Impl
{
    public class Deserializer : IDeserializer
    {
        private readonly JsonSerializer _serializer;

        public Deserializer()
        {
            _serializer = new JsonSerializer
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Include,
                DefaultValueHandling = DefaultValueHandling.Include,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };
        }

        public Deserializer(JsonSerializer serializer)
        {
            _serializer = serializer;
        }

        public T Deserialize<T>(string content)
        {
            using (var stringReader = new StringReader(content))
            {
                using (var jsonTextReader = new JsonTextReader(stringReader))
                {
                    return _serializer.Deserialize<T>(jsonTextReader);
                }
            }
        }
    }
}
