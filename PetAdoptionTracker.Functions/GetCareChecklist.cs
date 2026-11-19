using Azure;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace PetAdoptionTracker.Functions
{
    public class GetCareChecklist
    {
        [Function("GetCareChecklist")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")]
            HttpRequestData req)
        {
            string species = (req.Query["species"] ?? "")
                .Trim()
                .ToLowerInvariant();

            if (species != "dog" && species != "cat")
            {
                return TextResponse(
                    req,
                    HttpStatusCode.BadRequest,
                    "Enter species=dog or species=cat.");
            }

            string connectionString =
                Environment.GetEnvironmentVariable(
                    "AzureWebJobsStorage") ?? "";

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return TextResponse(
                    req,
                    HttpStatusCode.InternalServerError,
                    "The storage connection is not configured.");
            }

            try
            {
                var blob = new BlobClient(
                    connectionString,
                    "care-checklists",
                    $"{species}.txt");

                var download = await blob.DownloadContentAsync();

                string checklist = download.Value.Content.ToString();

                return TextResponse(
                    req,
                    HttpStatusCode.OK,
                    checklist);
            }
            catch (RequestFailedException ex)
            {
                return TextResponse(
                    req,
                    HttpStatusCode.InternalServerError,
                    $"Storage error: HTTP {ex.Status}, Code: {ex.ErrorCode}");
            }
        }

        private static HttpResponseData TextResponse(
            HttpRequestData req,
            HttpStatusCode status,
            string message)
        {
            var response = req.CreateResponse(status);

            response.Headers.Add(
                "Content-Type", "text/plain; charset=utf-8");

            response.WriteString(message);

            return response;
        }
    }
}