using System.ComponentModel.DataAnnotations;

namespace PetAdoptionTracker.Data.Models
{
    public class Adopter
    {
        public int AdopterId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(25)]
        public string Phone { get; set; } = string.Empty;
    }
}