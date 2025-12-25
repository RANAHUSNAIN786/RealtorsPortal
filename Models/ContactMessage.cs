using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealtorsPortal.Models
{
    public class ContactMessage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; } = "";

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = "";

        [StringLength(20)]
        public string Phone { get; set; } = "";

        [Required]
        [StringLength(500)]
        public string Subject { get; set; } = "";

        [Required]
        [StringLength(2000)]
        public string Message { get; set; } = "";

        [StringLength(50)]  // NEW: Add this
        public string Status { get; set; } = "Unread";  // Unread, Read, Replied

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        [StringLength(500)]
        public string Reply { get; set; } = "";  // Admin reply

        // Navigation (optional)
        public int? PropertyId { get; set; }

        public virtual Property? Property { get; set; }
    }
}