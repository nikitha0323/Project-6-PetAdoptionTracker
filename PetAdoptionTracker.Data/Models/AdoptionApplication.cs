using System;
using System.ComponentModel.DataAnnotations;

namespace PetAdoptionTracker.Data.Models
{
    public class AdoptionApplication
    {
        public int AdoptionApplicationId { get; set; }

        public int PetId { get; set; }

        public Pet? Pet { get; set; }

        public int AdopterId { get; set; }

        public Adopter? Adopter { get; set; }

        [DataType(DataType.Date)]
        public DateTime ApplicationDate { get; set; } = DateTime.Today;

        [Required]
        [StringLength(30)]
        public string Status { get; set; } = "Pending";
    }
}