using Company.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Company.Infrastructure.Data
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // ---------------- CREATE ROLES ----------------
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole("User"));

            // ---------------- CREATE ADMIN ----------------
            if (await userManager.FindByEmailAsync("admin@test.com") == null)
            {
                var admin = new AppUser
                {
                    DisplayName = "Admin",
                    Email = "admin@test.com",
                    UserName = "admin@test.com"
                };

                var result = await userManager.CreateAsync(admin, "Admin123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}