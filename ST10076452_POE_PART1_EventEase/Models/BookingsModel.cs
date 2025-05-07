using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10076452_POE_PART1_EventEase.Models
{
    public class BookingsModel
    {
        [Key]
        public int BookingID { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required]
        public int VenueId { get; set; }

        public DateTime BookingDate { get; set; }

        [ForeignKey("EventId")]
        public virtual EventsModel Events { get; set; }

        [ForeignKey("VenueId")]
        public virtual VenueModel Venues { get; set; }
    }
}
