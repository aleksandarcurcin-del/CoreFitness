using Microsoft.AspNetCore.Identity;

namespace CoreFitness2.Infrastructure.Seeds;

public class RoleSeeder
{
    public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
    {
        var roles = new[] { "Admin", "Member" };
        foreach (var role in roles)
        {
            var exists = await roleManager.RoleExistsAsync(role);
            if (!exists)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
