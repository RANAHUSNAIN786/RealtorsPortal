using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealtorsPortal.Models;
using System.ComponentModel.DataAnnotations.Schema;  // FIXED: For Column

namespace RealtorsPortal.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Property> Properties { get; set; } = null!;
        public DbSet<PropertyImage> PropertyImages { get; set; } = null!;
        public DbSet<Package> Packages { get; set; } = null!;
        public DbSet<UserPackage> UserPackages { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<ContactMessage> ContactMessages { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure relationships
            builder.Entity<Property>()
                .HasOne(p => p.User)
                .WithMany(u => u.Properties)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Property>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Properties)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Property>()
                .HasMany(p => p.Images)
                .WithOne(pi => pi.Property)
                .HasForeignKey(pi => pi.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserPackage>()
                .HasKey(up => up.Id);

            builder.Entity<UserPackage>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserPackages)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserPackage>()
                .HasOne(up => up.Package)
                .WithMany(p => p.UserPackages)
                .HasForeignKey(up => up.PackageId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Payment>()
                .HasOne(p => p.User)
                .WithMany(u => u.Payments)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Payment>()
                .HasOne(p => p.Package)
                .WithMany()
                .HasForeignKey(p => p.PackageId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<ContactMessage>()
                .HasOne(cm => cm.Property)
                .WithMany(p => p.ContactMessages)
                .HasForeignKey(cm => cm.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes for Performance
            builder.Entity<Property>()
                .HasIndex(p => p.UserId);

            builder.Entity<Property>()
                .HasIndex(p => p.CategoryId);

            builder.Entity<Property>()
                .HasIndex(p => p.IsApproved);

            builder.Entity<Property>()
                .HasIndex(p => p.CreatedAt);

            builder.Entity<Payment>()
                .HasIndex(p => p.UserId);

            builder.Entity<Payment>()
                .HasIndex(p => p.PaymentDate);

            builder.Entity<UserPackage>()
                .HasIndex(up => up.UserId);

            builder.Entity<UserPackage>()
                .HasIndex(up => up.ExpiryDate);

            builder.Entity<ContactMessage>()
                .HasIndex(cm => cm.Status);

            builder.Entity<ContactMessage>()
                .HasIndex(cm => cm.CreatedAt);

            // Seed Default Data - FIXED: No semicolons inside object initializers
            builder.Entity<Package>().HasData(
                new Package
                {
                    Id = 1,
                    Name = "Free Basic",
                    Price = 0,
                    DurationDays = 7,
                    MaxAds = 1,
                    MaxImagesPerAd = 3,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                },
                new Package
                {
                    Id = 2,
                    Name = "Premium",
                    Price = 29.99m,
                    DurationDays = 30,
                    MaxAds = 10,
                    MaxImagesPerAd = 10,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                }
            );

            builder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "Residential",
                    Description = "Houses, Apartments, Villas",
                    IconClass = "fas fa-home",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                },
                new Category
                {
                    Id = 2,
                    Name = "Commercial",
                    Description = "Shops, Offices, Land",
                    IconClass = "fas fa-building",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                }
            );

            // Decimal Precision - FIXED: No extra ;
            builder.Entity<Property>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Package>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");
        }
    }
}