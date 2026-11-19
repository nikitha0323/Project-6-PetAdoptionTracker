namespace PetAdoptionTracker.Api.ServiceModels
{
    public class AdopterServiceModel
    {
        public int AdopterId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;
    }
}