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
        public string Title { get; set; }

        [Required]
        [StringLength(5000)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public string PropertyType { get; set; } // House, Apartment, Shop, Land, Villa

        [Required]
        public string TransactionType { get; set; } // Sale, Rent, Lease

        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public double? Area { get; set; } // in square feet/meters

        [Required]
        public string Country { get; set; } = "Pakistan";

        [Required]
        public string City { get; set; }

        [Required]
        public string AreaName { get; set; } // Area/Locality
        public string Address { get; set; }

        public bool IsFeatured { get; set; } = false;
        public bool IsApproved { get; set; } = false;
        public int ViewCount { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ExpiryDate { get; set; }

        // Foreign Keys
        public string UserId { get; set; }
        public int CategoryId { get; set; }

        // Navigation Properties
        public virtual ApplicationUser User { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<PropertyImage> Images { get; set; }
    }
}