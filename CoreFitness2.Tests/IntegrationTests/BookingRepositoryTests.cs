using CoreFitness2.Domain.Entities.Bookings;
using CoreFitness2.Domain.Entities.Classes;
using CoreFitness2.Infrastructure.Data;
using CoreFitness2.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CoreFitness2.Tests.IntegrationTests;

public class BookingRepositoryTests
{
    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task AddAsync_ShouldSaveBookingToDatabase()
    {
        // Arrange
        await using var context = CreateDbContext();
        var repository = new BookingRepository(context);

        var gymClass = new GymClassEntity
        {
            Id = 1,
            Name = "Strength Class",
            Category = "Strength",
            Instructor = "Alex",
            StartTime = DateTime.UtcNow.AddDays(1),
            EndTime = DateTime.UtcNow.AddDays(1).AddHours(1),
            MaxParticipants = 10
        };

        context.GymClasses.Add(gymClass);
        await context.SaveChangesAsync();

        var booking = new BookingEntity
        {
            MemberId = 1,
            GymClassId = gymClass.Id,
            BookedAt = DateTime.UtcNow
        };

        // Act
        await repository.AddAsync(booking);
        await repository.SaveChangesAsync();

        // Assert
        var savedBooking = await context.Bookings.FirstOrDefaultAsync();

        Assert.NotNull(savedBooking);
        Assert.Equal(1, savedBooking.MemberId);
        Assert.Equal(gymClass.Id, savedBooking.GymClassId);
    }
}