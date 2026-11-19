namespace PetAdoptionTracker.Api.ServiceModels
{
    public class PetServiceModel
    {
        public int PetId { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Age { get; set; }

        public string Description { get; set; } = string.Empty;

        public string AdoptionStatus { get; set; } = string.Empty;

        public int ShelterId { get; set; }

        public int BreedId { get; set; }
    }
}