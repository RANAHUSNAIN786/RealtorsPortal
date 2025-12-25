using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealtorsPortal.Models;

namespace RealtorsPortal.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            try
            {
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

                // Ensure database is created
                await context.Database.EnsureCreatedAsync();

                // Create Roles
                string[] roleNames = { "Admin", "Agent", "PrivateSeller", "Visitor" };
                foreach (var roleName in roleNames)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                // Create Admin User
                var adminEmail = "admin@realtorsportal.com";
                if (await userManager.FindByEmailAsync(adminEmail) == null)
                {
                    var adminUser = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        FullName = "System Administrator",
                        UserType = "Admin",
                        EmailConfirmed = true,
                        PhoneNumber = "03001234567",
                        CreatedAt = DateTime.Now
                    };
                    var result = await userManager.CreateAsync(adminUser, "Admin@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                }

                // Create Test Agent
                var agentEmail = "agent@test.com";
                if (await userManager.FindByEmailAsync(agentEmail) == null)
                {
                    var agentUser = new ApplicationUser
                    {
                        UserName = agentEmail,
                        Email = agentEmail,
                        FullName = "Test Agent",
                        UserType = "Agent",
                        EmailConfirmed = true,
                        PhoneNumber = "03001234568",
                        CreatedAt = DateTime.Now
                    };
                    var result = await userManager.CreateAsync(agentUser, "Agent@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(agentUser, "Agent");
                    }
                }

                // Seed Categories if empty
                if (!await context.Categories.AnyAsync())
                {
                    var categories = new List<Category>
                    {
                        new Category { Name = "House", Description = "Residential houses", IconClass = "fas fa-house", IsActive = true, CreatedAt = DateTime.Now },
                        new Category { Name = "Apartment", Description = "Flats and apartments", IconClass = "fas fa-building", IsActive = true, CreatedAt = DateTime.Now },
                        new Category { Name = "Shop", Description = "Commercial shops", IconClass = "fas fa-store", IsActive = true, CreatedAt = DateTime.Now },
                        new Category { Name = "Office", Description = "Office spaces", IconClass = "fas fa-briefcase", IsActive = true, CreatedAt = DateTime.Now },
                        new Category { Name = "Land", Description = "Plots and land", IconClass = "fas fa-mountain", IsActive = true, CreatedAt = DateTime.Now },
                        new Category { Name = "Villa", Description = "Luxury villas", IconClass = "fas fa-archway", IsActive = true, CreatedAt = DateTime.Now }
                    };
                    await context.Categories.AddRangeAsync(categories);
                    await context.SaveChangesAsync();
                }

                // Seed Packages if empty - FIXED: Use MaxAds instead of AdLimit, add Description, IsActive, CreatedAt
                if (!await context.Packages.AnyAsync())
                {
                    var packages = new List<Package>
                    {
                        new Package { Name = "Basic", Description = "Free package for beginners", Price = 0, MaxAds = 1, MaxImagesPerAd = 3, DurationDays = 30, IsActive = true, CreatedAt = DateTime.Now },
                        new Package { Name = "Silver", Description = "Silver package for small users", Price = 1000, MaxAds = 5, MaxImagesPerAd = 5, DurationDays = 60, IsActive = true, CreatedAt = DateTime.Now },
                        new Package { Name = "Gold", Description = "Gold package for professionals", Price = 2500, MaxAds = 20, MaxImagesPerAd = 10, DurationDays = 90, IsActive = true, CreatedAt = DateTime.Now },
                        new Package { Name = "Platinum", Description = "Platinum package for enterprises", Price = 5000, MaxAds = 50, MaxImagesPerAd = 20, DurationDays = 120, IsActive = true, CreatedAt = DateTime.Now }
                    };
                    await context.Packages.AddRangeAsync(packages);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seed error: {ex.Message}");
            }
        }
    }
}