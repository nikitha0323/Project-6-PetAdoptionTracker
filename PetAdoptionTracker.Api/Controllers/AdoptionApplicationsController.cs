using Microsoft.AspNetCore.Mvc;
using PetAdoptionTracker.Api.Business;
using PetAdoptionTracker.Api.ServiceModels;
using PetAdoptionTracker.Data.Models;

namespace PetAdoptionTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdoptionApplicationsController : ControllerBase
    {
        private readonly BusinessManager _businessManager;

        public AdoptionApplicationsController(
            BusinessManager businessManager)
        {
            _businessManager = businessManager;
        }

        [HttpGet]
        public async Task<
            ActionResult<List<AdoptionApplicationServiceModel>>> GetAll()
        {
            var applications =
                await _businessManager.GetApplicationsAsync();

            var results = applications.Select(a =>
                new AdoptionApplicationServiceModel
                {
                    AdoptionApplicationId = a.AdoptionApplicationId,
                    PetId = a.PetId,
                    AdopterId = a.AdopterId,
                    ApplicationDate = a.ApplicationDate,
                    Status = a.Status
                }).ToList();

            return Ok(results);
        }

        [HttpGet("{id:int}")]
        public async Task<
            ActionResult<AdoptionApplicationServiceModel>> GetById(int id)
        {
            var application =
                await _businessManager.GetApplicationByIdAsync(id);

            if (application == null)
            {
                return NotFound();
            }

            return Ok(new AdoptionApplicationServiceModel
            {
                AdoptionApplicationId =
                    application.AdoptionApplicationId,
                PetId = application.PetId,
                AdopterId = application.AdopterId,
                ApplicationDate = application.ApplicationDate,
                Status = application.Status
            });
        }
        [HttpPost]
        public async Task<ActionResult<AdoptionApplicationServiceModel>> Save(
    AdoptionApplicationServiceModel model)
        {
            var application = new AdoptionApplication
            {
                AdoptionApplicationId = model.AdoptionApplicationId,
                PetId = model.PetId,
                AdopterId = model.AdopterId,
                ApplicationDate = model.ApplicationDate,
                Status = model.Status
            };

            try
            {
                var saved = await _businessManager
                    .SaveApplicationAsync(application);

                var result = new AdoptionApplicationServiceModel
                {
                    AdoptionApplicationId = saved.AdoptionApplicationId,
                    PetId = saved.PetId,
                    AdopterId = saved.AdopterId,
                    ApplicationDate = saved.ApplicationDate,
                    Status = saved.Status
                };

                if (model.AdoptionApplicationId == 0)
                {
                    return CreatedAtAction(
                        nameof(GetById),
                        new { id = saved.AdoptionApplicationId },
                        result);
                }

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _businessManager.DeleteApplicationAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}