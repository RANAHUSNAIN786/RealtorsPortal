using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealtorsPortal.Models
{
    public class PropertyImage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PropertyId { get; set; }

        [Required]
        [StringLength(500)]
        public string ImagePath { get; set; } = "";  // Full image path

        [StringLength(500)]
        public string ThumbnailPath { get; set; } = "";  // Thumbnail path (new for your req)

        [StringLength(100)]
        public string AltText { get; set; } = "";

        public int Order { get; set; } = 0;  // For sorting

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public virtual Property Property { get; set; } = null!;
    }
}