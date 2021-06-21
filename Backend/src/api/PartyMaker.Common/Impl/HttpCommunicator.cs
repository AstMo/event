using bgTeam;
using bgTeam.Extensions;
using PartyMaker.Common.Exceptions;
using Newtonsoft.Json;
using RestSharp;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace PartyMaker.Common.Impl
{
    public class HttpCommunicator : IHttpCommunicator
    {
        private readonly Dictionary<string, string> _headers;
        private readonly NewtonsoftJsonSerializer _serializer;
        private readonly IAppLogger _logger;
        private RestClient _client;
        private int _httpTimeout;

        public HttpCommunicator(IAppLogger logger)
        {
            _logger = logger;
            _headers = new Dictionary<string, string>();

            _serializer = new NewtonsoftJsonSerializer();
        }

        public void InitializeHost(string host)
        {
            var deserializer = new NewtonsoftJsonDeserializer();

            _httpTimeout = (int)TimeSpan.FromMinutes(10).TotalMilliseconds;
            _client = new RestClient(host)
            {
                ReadWriteTimeout = _httpTimeout,
                Timeout = _httpTimeout,
            };
            _client.AddHandler("application/json", () => { return deserializer; });
        }

        public string BuildUrl(string url, params object[] parameters)
        {
            return string.Format(url, parameters);
        }

        public void AddHeader(string header, string value)
        {
            if (_headers.ContainsKey(header))
            {
                _headers[header] = value;
            }
            else
            {
                _headers.Add(header, value);
            }
        }

        public string PerformRequest(string url, Method method, object body = null)
        {
            EnsureClienInitialized();

            var request = CreateRequest(url, method);
            request.Timeout = _httpTimeout;
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                if (body != null)
                {
                    request.AddJsonBody(body);
                }

                _headers.DoForEach(x => request.AddHeader(x.Key, x.Value));

                IRestResponse response;
                using (MiniProfiler.Current.Step($"Perform request {url}"))
                {
                    response = _client.Execute(request);
                }

                stopwatch.Stop();
                LogRequestResponse(url, method, body, request, response);

                if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created)
                {
                    ThrowSuzCommunicationException(response.StatusCode, response);
                }

                return response.Content;
            }
            catch (HttpCommunicationException e)
            {
                _logger.Warning(e.Message);
                throw;
            }
            finally
            {
                stopwatch.Stop();
            }
        }

        private void LogRequestResponse(
            string url,
            Method method,
            object body,
            IRestRequest request,
            IRestResponse response)
        {
            EnsureClienInitialized();

            var message = string.Format(
                "Request for endpoint {0} for url {1} (method {2}) with body {3} was performed and request params {4}. Response is ({5})  {6}",
                _client.BaseUrl,
                url,
                method,
                _serializer.Serialize(body),
                JsonConvert.SerializeObject(request.Parameters),
                response.StatusCode,
                response.Content);

            _logger.Info(message);
        }

        private void EnsureClienInitialized()
        {
            if (_client == null)
            {
                throw new ObjectNotInitializedException("InitializeHost must be called first");
            }
        }

        private IRestRequest CreateRequest(string resource, Method method)
        {
            var request = new RestRequest(resource, method)
            {
                JsonSerializer = _serializer,
            };

            return request;
        }

        private void ThrowSuzCommunicationException(HttpStatusCode responseStatusCode, IRestResponse response)
        {
            var errorMessage = EscapeMessage(response.ErrorMessage ?? response.StatusDescription);
            _logger.Warning(
                string.Format(
                    "Failed to perform request {0}. Server response: {1} ({2})",
                    response.Request.Resource,
                    response.StatusCode,
                    errorMessage));

            var exeption = GetErrorByStatusCode(responseStatusCode, response.Content, errorMessage);
            throw new HttpCommunicationException(responseStatusCode, exeption);
        }

        private string EscapeMessage(string message)
        {
            return message.Replace("{", string.Empty).Replace("}", string.Empty);
        }

        private string GetErrorByStatusCode(HttpStatusCode responseStatusCode, string reason, string errorMessage)
        {
            string result = string.Empty;
            switch (responseStatusCode)
            {
                case HttpStatusCode.BadRequest:
                    result = $"Wrong request. {reason} ";
                    break;
                case HttpStatusCode.Unauthorized:
                    result = "User unauthorized. ";
                    break;
                case HttpStatusCode.InternalServerError:
                    result = $"Internal server error {reason}";
                    break;
                default:
                    result = $"{errorMessage} with  content {reason}";
                    break;
            }

            return result;
        }
    }
}
