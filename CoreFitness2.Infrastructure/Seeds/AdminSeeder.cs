using CoreFitness2.Domain.Entities.Members;
using CoreFitness2.Infrastructure.Data;
using CoreFitness2.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CoreFitness2.Infrastructure.Seeds;

public class AdminSeeder
{
    public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager,ApplicationDbContext context)
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

        var memberExists = await context.Members
            .AnyAsync(x => x.ApplicationUserId == user.Id);

        if (!memberExists)
        {
            var member = new MemberEntity
            {
                ApplicationUserId = user.Id,
                Email = user.Email!,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Members.Add(member);
            await context.SaveChangesAsync();
        }
    }
}