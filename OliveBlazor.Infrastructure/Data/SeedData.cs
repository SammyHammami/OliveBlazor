using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OliveBlazor.Core.Domain.Accounts;
using OliveBlazor.Core.Domain.Security;
using OliveBlazor.Infrastructure.Indentity;

namespace OliveBlazor.Infrastructure.Data;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider, UserManager<UserIdentity> userManager, RoleManager<RoleIdentity> roleManager)
    {
        await CreateRolesAndUsers(serviceProvider, userManager, roleManager);
    }

    private static async Task CreateRolesAndUsers(IServiceProvider serviceProvider, UserManager<UserIdentity> userManager, RoleManager<RoleIdentity> roleManager)
    {
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        // Ensure the database is created
        context.Database.EnsureCreated();

        // Check if the Admin role exists
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            var permissions = GetAllPermissions();
            var oliveRole = new OliveRole()
            {
                Name = "Admin",
                Permissions = permissions

            };
            var role = new RoleIdentity()
            {
                Name = "Admin",
                Id = oliveRole.Id.ToString(),
                OliveRole = oliveRole
            };

          
            var result = await roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                var adminUser = new UserIdentity
                {
                    UserName = "Admin",
                    Email = "admin@example.com",
                    OliveUser = new OliveUser { FirstName = "Admin", LastName = "User" }
                };

                var userResult = await userManager.CreateAsync(adminUser, "Admin@123");

                if (userResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(adminUser);
                    await userManager.ConfirmEmailAsync(adminUser, code);

                }
            }
        }
    }

    private static Permissions GetAllPermissions()
    {
        return Enum.GetValues(typeof(Permissions)).Cast<Permissions>().Aggregate((p1, p2) => p1 | p2);
    }
}