using Microsoft.AspNetCore.Mvc;
using PetAdoptionTracker.Api.Business;
using PetAdoptionTracker.Api.ServiceModels;

namespace PetAdoptionTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BreedsController : ControllerBase
    {
        private readonly BusinessManager _businessManager;

        public BreedsController(BusinessManager businessManager)
        {
            _businessManager = businessManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<BreedServiceModel>>> GetAll()
        {
            var breeds = await _businessManager.GetBreedsAsync();

            var results = breeds.Select(b => new BreedServiceModel
            {
                BreedId = b.BreedId,
                Name = b.Name,
                Species = b.Species
            }).ToList();

            return Ok(results);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BreedServiceModel>> GetById(int id)
        {
            var breed = await _businessManager.GetBreedByIdAsync(id);

            if (breed == null)
            {
                return NotFound();
            }

            return Ok(new BreedServiceModel
            {
                BreedId = breed.BreedId,
                Name = breed.Name,
                Species = breed.Species
            });
        }
    }
}