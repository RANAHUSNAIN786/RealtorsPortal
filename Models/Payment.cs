using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealtorsPortal.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [StringLength(450)]
        public string? UserId { get; set; }  // Nullable if optional

        public int? PackageId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; } = 0;

        [Required]
        [StringLength(100)]
        public string PaymentMethod { get; set; } = "PayPal";  // FIXED: = "PayPal"

        [Required]
        [StringLength(100)]
        public string TransactionId { get; set; } = "";  // FIXED: = ""

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string PayerEmail { get; set; } = "";  // FIXED: = ""

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Pending";  // FIXED: = "Pending"

        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [StringLength(500)]
        public string Details { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        // FIXED: Nullable for navigation to avoid CS8618
        public virtual ApplicationUser? User { get; set; }

        public virtual Package? Package { get; set; }
    }
}