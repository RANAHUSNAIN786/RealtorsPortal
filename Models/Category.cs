using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RealtorsPortal.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }
        public string IconClass { get; set; } = "fas fa-home";
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public virtual ICollection<Property> Properties { get; set; }
    }
}