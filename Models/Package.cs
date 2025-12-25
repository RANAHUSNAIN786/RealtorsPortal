using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealtorsPortal.Models
{
    public class Package
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = "";

        [StringLength(500)]  // FIXED: Added Description
        public string Description { get; set; } = "";

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

        // Navigation
        public virtual ICollection<UserPackage> UserPackages { get; set; } = new List<UserPackage>();  // FIXED: Init to avoid CS8618
    }
}