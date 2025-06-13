using System.ComponentModel.DataAnnotations;

namespace ST10076452_POE_PART1_EventEase.Models
{
    public class EventType
    {
        [Key]
        public int EventTypeId { get; set; }

        [Required]
        public string? Name { get; set; }


        //Navigation properties
        public ICollection<EventsModel>? Events { get; set; }

    }
}
