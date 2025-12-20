using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;

namespace RealtorsPortal.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string UserType { get; set; } // Admin, Agent, PrivateSeller
        public string Address { get; set; }
        public string PhoneNumber2 { get; set; }
        public string ProfilePicture { get; set; } = "/images/default-profile.png";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public virtual ICollection<Property> Properties { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<UserPackage> UserPackages { get; set; }
    }
}