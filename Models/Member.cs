using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealtorsPortal.Models
{
    public class Member  // Ya agar Identity use kar rahe ho, to yeh extend na karo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; } = "";

        [Required]
        [StringLength(100)]
        public string Password { get; set; } = "";  // Hash in real

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = "";

        [StringLength(20)]
        public string Phone { get; set; } = "";

        [StringLength(50)]
        public string Type { get; set; } = "Seller";  // Seller/Agent/Admin

        public bool IsActive { get; set; } = true;

        [StringLength(500)]
        public string ProfileDescription { get; set; } = "";

        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public int? SubscriptionPackageId { get; set; }

        // Navigation
        public virtual SubscriptionPackage? SubscriptionPackage { get; set; }

        public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}