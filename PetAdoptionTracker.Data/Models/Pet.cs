using System.ComponentModel.DataAnnotations;

namespace PetAdoptionTracker.Data.Models
{
    public class Pet
    {
        public int PetId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(0, 100)]
        public int Age { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string AdoptionStatus { get; set; } = "Available";

        public int ShelterId { get; set; }

        public Shelter? Shelter { get; set; }

        public int BreedId { get; set; }

        public Breed? Breed { get; set; }
    }
}