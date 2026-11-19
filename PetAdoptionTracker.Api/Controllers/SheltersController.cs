using Microsoft.AspNetCore.Mvc;
using PetAdoptionTracker.Api.Business;
using PetAdoptionTracker.Api.ServiceModels;

namespace PetAdoptionTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SheltersController : ControllerBase
    {
        private readonly BusinessManager _businessManager;

        public SheltersController(BusinessManager businessManager)
        {
            _businessManager = businessManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<ShelterServiceModel>>> GetAll()
        {
            var shelters = await _businessManager.GetSheltersAsync();

            var results = shelters.Select(s => new ShelterServiceModel
            {
                ShelterId = s.ShelterId,
                Name = s.Name,
                Address = s.Address,
                Phone = s.Phone
            }).ToList();

            return Ok(results);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ShelterServiceModel>> GetById(int id)
        {
            var shelter = await _businessManager.GetShelterByIdAsync(id);

            if (shelter == null)
            {
                return NotFound();
            }

            return Ok(new ShelterServiceModel
            {
                ShelterId = shelter.ShelterId,
                Name = shelter.Name,
                Address = shelter.Address,
                Phone = shelter.Phone
            });
        }
    }
}