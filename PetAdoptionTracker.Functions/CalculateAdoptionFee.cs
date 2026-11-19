using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace PetAdoptionTracker.Functions
{
    public class CalculateAdoptionFee
    {
        private readonly ILogger _logger;

        public CalculateAdoptionFee(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory
                .CreateLogger<CalculateAdoptionFee>();
        }

        [Function("CalculateAdoptionFee")]
        public HttpResponseData Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")]
            HttpRequestData req)
        {
            _logger.LogInformation("Calculating adoption fee.");

            string ageText = req.Query["age"] ?? string.Empty;
            if (!int.TryParse(ageText, out int age)
                || age < 0 || age > 100)
            {
                var error = req.CreateResponse(
                    HttpStatusCode.BadRequest);

                error.Headers.Add(
                    "Content-Type", "text/plain; charset=utf-8");

                error.WriteString(
                    "Enter a whole-number age between 0 and 100.");

                return error;
            }

            decimal fee;

            if (age <= 1)
            {
                fee = 150;
            }
            else if (age <= 6)
            {
                fee = 100;
            }
            else
            {
                fee = 50;
            }

            var response = req.CreateResponse(HttpStatusCode.OK);

            response.Headers.Add(
                "Content-Type", "text/plain; charset=utf-8");

            response.WriteString(
                $"Estimated adoption fee for a pet aged {age}: ${fee:0.00}");

            return response;
        }
    }
}