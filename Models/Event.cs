using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
    public class Event
    {
        public int EventId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        // Event image stored in Azure Blob Storage
        public string? ImageUrl { get; set; }

        // Foreign key for Venue
        public int VenueId { get; set; }

        public Venue? Venue { get; set; }

        // Foreign key for EventType
        [Display(Name = "Event Type")]
        public int EventTypeId { get; set; }

        public EventType? EventType { get; set; }

        public ICollection<Booking>? Bookings { get; set; }
    }
}