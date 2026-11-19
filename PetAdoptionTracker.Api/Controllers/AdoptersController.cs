using Microsoft.AspNetCore.Mvc;
using PetAdoptionTracker.Api.Business;
using PetAdoptionTracker.Api.ServiceModels;
using PetAdoptionTracker.Data.Models;

namespace PetAdoptionTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdoptersController : ControllerBase
    {
        private readonly BusinessManager _businessManager;

        public AdoptersController(BusinessManager businessManager)
        {
            _businessManager = businessManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<AdopterServiceModel>>> GetAll()
        {
            var adopters = await _businessManager.GetAdoptersAsync();

            var results = adopters.Select(a => new AdopterServiceModel
            {
                AdopterId = a.AdopterId,
                Name = a.Name,
                Email = a.Email,
                Phone = a.Phone
            }).ToList();

            return Ok(results);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AdopterServiceModel>> GetById(int id)
        {
            var adopter = await _businessManager.GetAdopterByIdAsync(id);

            if (adopter == null)
            {
                return NotFound();
            }

            return Ok(new AdopterServiceModel
            {
                AdopterId = adopter.AdopterId,
                Name = adopter.Name,
                Email = adopter.Email,
                Phone = adopter.Phone
            });

        }

        [HttpPost]
        public async Task<ActionResult<AdopterServiceModel>> Save(
    AdopterServiceModel model)
        {
            var adopter = new Adopter
            {
                AdopterId = model.AdopterId,
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone
            };

            try
            {
                var saved = await _businessManager.SaveAdopterAsync(adopter);

                var result = new AdopterServiceModel
                {
                    AdopterId = saved.AdopterId,
                    Name = saved.Name,
                    Email = saved.Email,
                    Phone = saved.Phone
                };

                if (model.AdopterId == 0)
                {
                    return CreatedAtAction(
                        nameof(GetById),
                        new { id = saved.AdopterId },
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
            try
            {
                var deleted = await _businessManager.DeleteAdopterAsync(id);

                if (!deleted)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}