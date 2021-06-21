using bgTeam;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace PartyMaker.Application.Middleware
{
    public class LoggerMiddleware
    {
        private const int ReadChunkBufferLength = 4096;

        private readonly RequestDelegate _next;

        private readonly IAppLogger _logger;

        public LoggerMiddleware(RequestDelegate next, IAppLogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = await FormatRequest(context.Request);

            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                var response = await FormatResponse(context.Response);

                _logger?.Debug($"Request is: {request.Substring(0, request.Length > 500 ? 500 : request.Length)} \n Response is: {response.Substring(0, response.Length > 500 ? 500 : response.Length)}");

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            request.EnableBuffering();

            using (var requestStream = new MemoryStream())
            {
                await request.Body.CopyToAsync(requestStream);
                request.Body.Seek(0, SeekOrigin.Begin);
                return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {ReadStreamInChunks(requestStream)}";
            }
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);

            string text = await new StreamReader(response.Body).ReadToEndAsync();

            response.Body.Seek(0, SeekOrigin.Begin);

            return $"{response.StatusCode}: {text}";
        }

        private string ReadStreamInChunks(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            string result;
            using (var textWriter = new StringWriter())
            using (var reader = new StreamReader(stream))
            {
                var readChunk = new char[ReadChunkBufferLength];
                int readChunkLength;

                do
                {
                    readChunkLength = reader.ReadBlock(readChunk, 0, ReadChunkBufferLength);
                    textWriter.Write(readChunk, 0, readChunkLength);
                }
                while (readChunkLength > 0);

                result = textWriter.ToString();
            }

            return result;
        }
    }
}
