using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealtorsPortal.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public string PaymentMethod { get; set; } // PayPal, BankTransfer, JazzCash, etc.

        public string TransactionId { get; set; }
        public string PayerEmail { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Completed, Failed, Refunded

        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public DateTime? CompletedDate { get; set; }

        // Foreign Keys
        public string UserId { get; set; }
        public int? PackageId { get; set; }

        // Navigation
        public virtual ApplicationUser User { get; set; }
        public virtual Package Package { get; set; }
    }
}