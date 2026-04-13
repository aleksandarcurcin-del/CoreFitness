using CoreFitness2.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace CoreFitness2.Infrastructure.Seeds;

public static class AdminSeeder
{
    public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
    {
        var email = "admin@corefitness.com";
        var password = "Admin123!";

        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email
            };

            var createResult = await userManager.CreateAsync(user, password);

            if (!createResult.Succeeded)
                return;
        }

        if (!await userManager.IsInRoleAsync(user, "Admin"))
        {
            await userManager.AddToRoleAsync(user, "Admin");
        }
    }
}