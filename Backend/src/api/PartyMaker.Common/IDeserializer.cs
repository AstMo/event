namespace PartyMaker.Common
{
    public interface IDeserializer
    {
        T Deserialize<T>(string content);
    }
}
