using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10076452_POE_PART1_EventEase.Models
{
    public class BookingsModel
    {
        public int BookingID { get; set; }

        public int EventId { get; set; }
        public virtual EventsModel Events { get; set; }

        public int VenueId { get; set; }
        public virtual VenueModel Venues { get; set; }

        public DateTime BookingDate { get; set; }
    }
}
