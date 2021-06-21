using RestSharp;

namespace PartyMaker.Common
{
    public interface IHttpCommunicator
    {
        void InitializeHost(string host);

        string BuildUrl(string url, params object[] parameters);

        void AddHeader(string header, string value);

        string PerformRequest(string url, Method method, object body = null);
    }
}
