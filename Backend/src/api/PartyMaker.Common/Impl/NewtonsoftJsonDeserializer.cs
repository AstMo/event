using Newtonsoft.Json;
using RestSharp;

namespace PartyMaker.Common.Impl
{
    public class NewtonsoftJsonDeserializer : Deserializer, RestSharp.Deserializers.IDeserializer
    {
        public NewtonsoftJsonDeserializer()
            : base()
        {
        }

        public NewtonsoftJsonDeserializer(JsonSerializer serializer)
            : base(serializer)
        {
        }

        public string RootElement { get; set; }

        public string Namespace { get; set; }

        public string DateFormat { get; set; }

        public T Deserialize<T>(IRestResponse response)
        {
            return Deserialize<T>(response.Content);
        }
    }
}
