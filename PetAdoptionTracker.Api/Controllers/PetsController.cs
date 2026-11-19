using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetAdoptionTracker.Api.Business;
using PetAdoptionTracker.Api.ServiceModels;
using PetAdoptionTracker.Data.Models;

namespace PetAdoptionTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly BusinessManager _businessManager;

        public PetsController(BusinessManager businessManager)
        {
            _businessManager = businessManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<PetServiceModel>>> GetAll()
        {
            var pets = await _businessManager.GetPetsAsync();

            var results = pets.Select(p => new PetServiceModel
            {
                PetId = p.PetId,
                Name = p.Name,
                Age = p.Age,
                Description = p.Description,
                AdoptionStatus = p.AdoptionStatus,
                ShelterId = p.ShelterId,
                BreedId = p.BreedId
            }).ToList();

            return Ok(results);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PetServiceModel>> GetById(int id)
        {
            var pet = await _businessManager.GetPetByIdAsync(id);

            if (pet == null)
            {
                return NotFound();
            }

            var result = new PetServiceModel
            {
                PetId = pet.PetId,
                Name = pet.Name,
                Age = pet.Age,
                Description = pet.Description,
                AdoptionStatus = pet.AdoptionStatus,
                ShelterId = pet.ShelterId,
                BreedId = pet.BreedId
            };

            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<PetServiceModel>> Save(
    PetServiceModel model)
        {
            var pet = new Pet
            {
                PetId = model.PetId,
                Name = model.Name,
                Age = model.Age,
                Description = model.Description,
                AdoptionStatus = model.AdoptionStatus,
                ShelterId = model.ShelterId,
                BreedId = model.BreedId
            };

            try
            {
                var saved = await _businessManager.SavePetAsync(pet);

                var result = new PetServiceModel
                {
                    PetId = saved.PetId,
                    Name = saved.Name,
                    Age = saved.Age,
                    Description = saved.Description,
                    AdoptionStatus = saved.AdoptionStatus,
                    ShelterId = saved.ShelterId,
                    BreedId = saved.BreedId
                };

                if (model.PetId == 0)
                {
                    return CreatedAtAction(
                        nameof(GetById),
                        new { id = saved.PetId },
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
                var deleted = await _businessManager.DeletePetAsync(id);

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