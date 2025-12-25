using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealtorsPortal.Models
{
    public class UserPackage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(450)]
        public string UserId { get; set; } = "";  // FIXED: = ""

        [Required]
        public int PackageId { get; set; }

        public DateTime StartDate { get; set; } = DateTime.Now;

        public DateTime? ExpiryDate { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        // FIXED: Nullable for navigation
        public virtual ApplicationUser? User { get; set; }

        public virtual Package? Package { get; set; }
    }
}