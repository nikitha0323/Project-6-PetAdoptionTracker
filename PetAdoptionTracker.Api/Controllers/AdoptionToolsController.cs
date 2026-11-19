using Microsoft.AspNetCore.Mvc;
using PetAdoptionTracker.Api.Business;

namespace PetAdoptionTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdoptionToolsController : ControllerBase
    {
        private readonly BusinessManager _businessManager;

        public AdoptionToolsController(BusinessManager businessManager)
        {
            _businessManager = businessManager;
        }

        [HttpGet("adoption-fee")]
        public async Task<IActionResult> GetAdoptionFee(
            [FromQuery] int age)
        {
            try
            {
                var result = await _businessManager
                    .CalculateAdoptionFeeAsync(age);

                return Content(result, "text/plain");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (HttpRequestException)
            {
                return StatusCode(502, new
                {
                    message = "Could not retrieve the fee from Azure."
                });
            }
            catch (TaskCanceledException)
            {
                return StatusCode(504, new
                {
                    message = "The Azure request timed out."
                });
            }
        }

        [HttpGet("care-checklist")]
        public async Task<IActionResult> GetCareChecklist(
            [FromQuery] string species)
        {
            try
            {
                var result = await _businessManager
                    .GetCareChecklistAsync(species);

                return Content(result, "text/plain");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (HttpRequestException)
            {
                return StatusCode(502, new
                {
                    message = "Could not retrieve the checklist from Azure."
                });
            }
            catch (TaskCanceledException)
            {
                return StatusCode(504, new
                {
                    message = "The Azure request timed out."
                });
            }
        }
    }
}