using System.ComponentModel.DataAnnotations;


namespace ST10076452_POE_PART1_EventEase.Models
{
    public class VenueModel
    {



            [Key] // Primary Key
            public int VenueId { get; set; }

            [Required]
            [StringLength(150)]
            public string VenueName { get; set; }

            [Required]
            [StringLength(250)]
            public string Location { get; set; }

            [Range(1, 100000, ErrorMessage = "Capacity must be at least 1.")]
            public int Capacity { get; set; }

            [Url]
            [StringLength(500)]
            public string ImageUrl { get; set; }


    }
    }


