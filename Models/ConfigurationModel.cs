using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealtorsPortal.Models
{
    public class ConfigurationModel
    {
        [Key]
        public int Id { get; set; } = 1;

        [Required]
        [Range(1, 365)]
        public int AdExpirationDays { get; set; } = 30;

        [Required]
        [Range(1, 100)]
        public int AdsPerPage { get; set; } = 10;

        [Required]
        [StringLength(5)]
        public string CurrencySymbol { get; set; } = "$";

        public bool EnableFeaturedAds { get; set; } = true;

        public bool EnableExtendedAds { get; set; } = true;

        [StringLength(100)]
        public string PayPalClientId { get; set; } = "";

        [StringLength(100)]
        public string PayPalSecret { get; set; } = "";

        [Required]
        [StringLength(255)]
        public string AdminPassword { get; set; } = "DefaultPass123";  // Hash in real

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }
    }
}