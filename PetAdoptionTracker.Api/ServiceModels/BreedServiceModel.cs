namespace PetAdoptionTracker.Api.ServiceModels
{
    public class BreedServiceModel
    {
        public int BreedId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Species { get; set; } = string.Empty;
    }
}