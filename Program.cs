#nullable disable
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using RealtorsPortal.Data;
using RealtorsPortal.Models;
using System.IO;

#nullable disable // Disable nullable warnings for now

var builder = WebApplication.CreateBuilder(args);

// ============================================
// LOAD ENVIRONMENT VARIABLES
// ============================================
var envFilePath = Path.Combine(Directory.GetCurrentDirectory(), ".env.local");
if (File.Exists(envFilePath))
{
    Console.WriteLine($"📁 Loading environment from: {envFilePath}");
    foreach (var line in File.ReadAllLines(envFilePath))
    {
        if (!string.IsNullOrWhiteSpace(line) && !line.TrimStart().StartsWith("#"))
        {
            var parts = line.Split('=', 2);
            if (parts.Length == 2)
            {
                var key = parts[0].Trim();
                var value = parts[1].Trim();
                if (value.StartsWith("\"") && value.EndsWith("\""))
                    value = value.Substring(1, value.Length - 2);
                Environment.SetEnvironmentVariable(key, value);
            }
        }
    }
}

// ============================================
// CONFIGURATION SETUP
// ============================================
builder.Configuration.AddEnvironmentVariables();

// ============================================
// DATABASE CONFIGURATION
// ============================================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Server=(localdb)\\mssqllocaldb;Database=RealtorsPortalDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// ============================================
// IDENTITY CONFIGURATION
// ============================================
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// ============================================
// COOKIE AND SESSION
// ============================================
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.SlidingExpiration = true;
});
builder.Services.AddSession();

// ============================================
// APPLICATION SERVICES
// ============================================
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// ============================================
// REQUEST PIPELINE
// ============================================
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// ============================================
// STATIC FILES
// ============================================
app.UseStaticFiles();

var userPath = Path.Combine(app.Environment.WebRootPath, "user");
if (!Directory.Exists(userPath))
    Directory.CreateDirectory(userPath);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(userPath),
    RequestPath = "/user"
});

var uploadsPath = Path.Combine(app.Environment.WebRootPath, "uploads");
if (!Directory.Exists(uploadsPath))
    Directory.CreateDirectory(uploadsPath);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/uploads"
});

app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// ============================================
// DATABASE SEEDING
// ============================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        await context.Database.EnsureCreatedAsync();

        // Create roles
        string[] roleNames = { "Admin", "Agent", "PrivateSeller", "Visitor" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Create admin user
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

        // Create test agent
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

        // Seed categories if empty
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
        }

        // FIXED: Seed packages if empty (add this section)
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
        }

        await context.SaveChangesAsync();  // FIXED: Save after all seeds
        Console.WriteLine("Database seeded successfully!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Seed error: {ex.Message}");
    }
}

// ============================================
// ROUTES
// ============================================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();