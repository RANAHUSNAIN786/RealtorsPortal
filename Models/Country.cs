// Country.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RealtorsPortal.Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = "";

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Region> Regions { get; set; } = new List<Region>();
    }
}