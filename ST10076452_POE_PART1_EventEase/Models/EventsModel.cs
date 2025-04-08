using System;
using System.ComponentModel.DataAnnotations;

namespace ST10076452_POE_PART1_EventEase.Models
{
    public class EventsModel
    {
        [Key] // Primary Key
        public int EventId { get; set; }

        [Required]
        [StringLength(150)]
        public string EventName { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public int VenueId { get; set; } // Foreign Key linking to Venue
    }
}
