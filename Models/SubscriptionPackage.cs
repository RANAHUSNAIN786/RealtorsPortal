using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealtorsPortal.Models
{
    public class SubscriptionPackage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = "";  // e.g., "Basic", "Premium"

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } = 0;

        [Required]
        [Range(1, 365)]
        public int DurationDays { get; set; } = 30;

        [Required]
        [Range(1, 100)]
        public int MaxAds { get; set; } = 5;

        [Required]
        [Range(1, 20)]
        public int MaxImagesPerAd { get; set; } = 5;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        // Navigation (if needed for members)
        public virtual ICollection<Member> Members { get; set; } = new List<Member>();
    }
}