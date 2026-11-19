namespace PetAdoptionTracker.Api.ServiceModels
{
    public class AdoptionApplicationServiceModel
    {
        public int AdoptionApplicationId { get; set; }

        public int PetId { get; set; }

        public int AdopterId { get; set; }

        public DateTime ApplicationDate { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}