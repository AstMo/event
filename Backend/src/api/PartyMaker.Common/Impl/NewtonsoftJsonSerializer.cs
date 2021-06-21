namespace PartyMaker.Common.Impl
{
    using Newtonsoft.Json;

    public class NewtonsoftJsonSerializer : Serializer, RestSharp.Serializers.ISerializer
    {
        public NewtonsoftJsonSerializer()
        {
            ContentType = "application/json";
        }

        public NewtonsoftJsonSerializer(JsonSerializer serializer)
            : base(serializer)
        {
            ContentType = "application/json";
        }

        public string DateFormat { get; set; }

        public string RootElement { get; set; }

        public string Namespace { get; set; }

        public string ContentType { get; set; }
    }
}
