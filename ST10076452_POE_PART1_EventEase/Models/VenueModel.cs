using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ST10076452_POE_PART1_EventEase.Models
{
    public class VenueModel
    {
        [Key]
        public int VenueId { get; set; }

        [Required]
        [StringLength(150)]
        public string VenueName { get; set; }

        [Required]
        [StringLength(250)]
        public string Location { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        [StringLength(500)]
        public string ImageUrl { get; set; }

        // New field for availability
        [Display(Name = "Is Available?")]
        public bool IsAvailable { get; set; } = true;

        // Navigation properties
        public virtual ICollection<EventsModel> Events { get; set; }
        public virtual ICollection<BookingsModel> Bookings { get; set; }
    }
}
