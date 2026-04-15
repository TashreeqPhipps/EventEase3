using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
    public class Venue
    {
        public int VenueId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        public string Location { get; set; }

        [Range(1, 100000)]
        public int Capacity { get; set; }

        public string? ImageUrl { get; set; }

        // Navigation property
        public ICollection<Event>? Events { get; set; }
    }
}