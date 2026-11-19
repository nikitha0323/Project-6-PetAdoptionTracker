using System.ComponentModel.DataAnnotations;

namespace PetAdoptionTracker.Data.Models
{
    public class Shelter
    {
        public int ShelterId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(25)]
        public string Phone { get; set; } = string.Empty;
    }
}