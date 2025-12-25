using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealtorsPortal.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = "";  // FIXED: = "" for CS8618

        [Required]
        [StringLength(50)]
        public string UserType { get; set; } = "Seller";  // FIXED: = "Seller"

        [StringLength(500)]
        public string Address { get; set; } = "";  // FIXED: = ""

        [StringLength(20)]
        public string PhoneNumber2 { get; set; } = "";  // FIXED: = ""

        public bool IsActive { get; set; } = true;

        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;  // For seed

        // Navigation - FIXED: Init lists for CS8618
        public virtual ICollection<Property> Properties { get; set; } = new List<Property>();

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

        public virtual ICollection<UserPackage> UserPackages { get; set; } = new List<UserPackage>();
    }
}