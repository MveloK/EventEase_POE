using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10076452_POE_PART1_EventEase.Models
{
    public class EventsModel
    {
        [Key]
        public int EventId { get; set; }

        [Required]
        [StringLength(150)]
        public string EventName { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public int VenueId { get; set; }

        [ForeignKey("VenueId")]
        public virtual VenueModel Venue { get; set; }

        // New field for EventType
        [Required(ErrorMessage = "Event type is required.")]
        public int EventTypeId { get; set; }

        [ForeignKey("EventTypeId")]
        public virtual EventType? EventType { get; set; }

        public virtual ICollection<BookingsModel> Bookings { get; set; }
    }
}
