namespace RealtorsPortal.Models
{
    public class PropertyImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public bool IsPrimary { get; set; } = false;
        public DateTime UploadedAt { get; set; } = DateTime.Now;

        public int PropertyId { get; set; }
        public virtual Property Property { get; set; }
    }
}