namespace RealtorsPortal.Models
{
    public class UserPackage
    {
        public int Id { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.Now;
        public DateTime ExpiryDate { get; set; }
        public int AdsPosted { get; set; } = 0;
        public bool IsActive { get; set; } = true;

        // Foreign Keys
        public string UserId { get; set; }
        public int PackageId { get; set; }

        // Navigation
        public virtual ApplicationUser User { get; set; }
        public virtual Package Package { get; set; }
    }
}