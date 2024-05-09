using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace bbFunc
{
    public class HttpHello
    {
        private readonly ILogger _logger;

        public HttpHello(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HttpHello>();
        }

        [Function("HttpHello")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");


            var name = req.Query["name"];

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            string message = "Welcome to Azure Functions";

            if (!string.IsNullOrEmpty(name))
            {
                message = message + $", {name}!";
            }

            response.WriteString(message);

            return response;
        }
    }
}
