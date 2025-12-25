using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealtorsPortal.Models
{
    public class Property
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = "";

        [Required]
        [StringLength(5000)]
        public string Description { get; set; } = "";

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } = 0;

        [Required]
        [StringLength(50)]
        public string PropertyType { get; set; } = "";  // House, Apartment, Shop, Land, Villa

        [Required]
        [StringLength(50)]
        public string TransactionType { get; set; } = "";  // Sale, Rent, Lease

        public int? Bedrooms { get; set; }

        public int? Bathrooms { get; set; }

        public double? Area { get; set; }  // in square feet/meters

        [Required]
        [StringLength(100)]
        public string Country { get; set; } = "Pakistan";

        [Required]
        [StringLength(100)]
        public string City { get; set; } = "";

        [Required]
        [StringLength(100)]
        public string AreaName { get; set; } = "";  // Area/Locality

        [StringLength(500)]
        public string Address { get; set; } = "";

        public bool IsFeatured { get; set; } = false;

        public bool IsApproved { get; set; } = false;

        public int ViewCount { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public DateTime? ExpiryDate { get; set; }

        // Foreign Keys
        [StringLength(450)]
        public string? UserId { get; set; }

        public int CategoryId { get; set; }

        // Navigation Properties
        public virtual ApplicationUser? User { get; set; }

        public virtual Category Category { get; set; } = null!;

        public virtual ICollection<PropertyImage> Images { get; set; } = new List<PropertyImage>();

        public virtual ICollection<ContactMessage> ContactMessages { get; set; } = new List<ContactMessage>();  // NEW: Add this
    }
}