using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        [StringLength(50)]
        public string BookingReference { get; set; }

        // Foreign key
        public int EventId { get; set; }

        public Event? Event { get; set; }
    }
}