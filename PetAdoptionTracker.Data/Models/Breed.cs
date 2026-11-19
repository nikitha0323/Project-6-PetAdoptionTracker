using System.ComponentModel.DataAnnotations;

namespace PetAdoptionTracker.Data.Models
{
    public class Breed
    {
        public int BreedId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Species { get; set; } = string.Empty;
    }
}