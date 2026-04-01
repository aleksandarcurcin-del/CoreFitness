using CoreFitness2.Domain.Entities.Classes;
using CoreFitness2.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace CoreFitness2.Infrastructure.Seeds;

public class GymClassSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.GymClasses.AnyAsync())
            return;

        var classes = new List<GymClassEntity>
        {
            new()
            {
                Name = "Core",
                Description = "Strengthen your core with targeted exercises for abs and lower back.",
                Category = "Core",
                Instructor = "Hans Svensson",
                StartTime = DateTime.Today.AddDays(1).AddHours(18),
                EndTime = DateTime.Today.AddDays(1).AddHours(19),
                MaxParticipants = 15
            },
            new()
            {
                Name = "HIIT Blast",
                Description = "High intensity interval training for endurance and strength.",
                Category = "Cardio",
                Instructor = "Joachim Larsson",
                StartTime = DateTime.Today.AddDays(2).AddHours(17),
                EndTime = DateTime.Today.AddDays(2).AddHours(18),
                MaxParticipants = 20
            },
            new()
            {
                Name = "Upper Body Strength",
                Description = "Focus on building strength in chest, shoulders, arms, and back.",
                Category = "Strength",
                Instructor = "Emil Nilsson",
                StartTime = DateTime.Today.AddDays(3).AddHours(16),
                EndTime = DateTime.Today.AddDays(3).AddHours(17),
                MaxParticipants = 12
            }
        };

        await context.GymClasses.AddRangeAsync(classes);
        await context.SaveChangesAsync();
    }
}
