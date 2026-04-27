using CoreFitness2.Application.Dtos.Bookings;
using CoreFitness2.Application.Interfaces;
using CoreFitness2.Application.Services;
using CoreFitness2.Domain.Entities.Bookings;
using CoreFitness2.Domain.Entities.Classes;
using Moq;
using System.Linq.Expressions;

namespace CoreFitness2.Tests.UnitTests;

public class BookingServiceTests
{
    private readonly Mock<IBookingRepository> _bookingRepositoryMock;
    private readonly Mock<IGymClassRepository> _gymClassRepositoryMock;
    private readonly BookingService _bookingService;


    public BookingServiceTests()
    {
        _bookingRepositoryMock = new Mock<IBookingRepository>();
        _gymClassRepositoryMock = new Mock<IGymClassRepository>();
        _bookingService = new BookingService(_bookingRepositoryMock.Object, _gymClassRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateBookingAsync_ShouldFail_WhenGymClassDoesNotExist()
    {
        // Arrange
        var dto = new CreateBookingDto
        {
            MemberId = 1,
            GymClassId = 10
        };

        _gymClassRepositoryMock
            .Setup(x => x.GetOneAsync(
                It.IsAny<Expression<Func<GymClassEntity, bool>>>(),
                false,
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<GymClassEntity, object>>[]>()))
            .ReturnsAsync((GymClassEntity?)null);

        // Act
        var result = await _bookingService.CreateBookingAsync(dto);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Equal("The selected class could not be found.", result.ErrorMessage);

        _bookingRepositoryMock.Verify(x => x.AddAsync(It.IsAny<BookingEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        _bookingRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateBookingAsync_ShouldFail_WhenMemberAlreadyBookedClass()
    {
        // Arrange
        var dto = new CreateBookingDto
        {
            MemberId = 1,
            GymClassId = 10
        };

        var gymClass = new GymClassEntity
        {
            Id = 10,
            Name = "Strength",
            Category = "Strength",
            Instructor = "Alex",
            StartTime = DateTime.UtcNow.AddDays(1),
            EndTime = DateTime.UtcNow.AddDays(1).AddHours(1),
            MaxParticipants = 10
        };

        _gymClassRepositoryMock
            .Setup(x => x.GetOneAsync(
                It.IsAny<Expression<Func<GymClassEntity, bool>>>(),
                false,
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<GymClassEntity, object>>[]>()))
            .ReturnsAsync(gymClass);

        _bookingRepositoryMock
            .Setup(x => x.ExistsAsync(
                It.IsAny<Expression<Func<BookingEntity, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _bookingService.CreateBookingAsync(dto);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Equal("You have already booked this class.", result.ErrorMessage);

        _bookingRepositoryMock.Verify(x => x.AddAsync(It.IsAny<BookingEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        _bookingRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }


    [Fact]
    public async Task CreateBookingAsync_ShouldFail_WhenGymClassIsFull()
    {
        var dto = new CreateBookingDto
        {
            MemberId = 1,
            GymClassId = 10
        };

        var gymClass = new GymClassEntity
        {
            Id = 10,
            Name = "Cardio",
            Category = "Conditioning",
            Instructor = "Emma",
            StartTime = DateTime.UtcNow.AddDays(1),
            EndTime = DateTime.UtcNow.AddDays(1).AddHours(1),
            MaxParticipants = 1,
            Bookings =
            [
                new BookingEntity
                {
                    Id = 1,
                    MemberId = 2,
                    GymClassId = 10
                }
            ]
        };

        _gymClassRepositoryMock
            .Setup(x => x.GetOneAsync(
                It.IsAny<Expression<Func<GymClassEntity, bool>>>(),
                false,
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<GymClassEntity, object>>[]>()))
            .ReturnsAsync(gymClass);

        _bookingRepositoryMock
            .Setup(x => x.ExistsAsync(
                It.IsAny<Expression<Func<BookingEntity, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _bookingService.CreateBookingAsync(dto);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Equal("This class is already full.", result.ErrorMessage);

        _bookingRepositoryMock.Verify(x => x.AddAsync(It.IsAny<BookingEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        _bookingRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }


    [Fact]
    public async Task CreateBookingAsync_ShouldSucceed_WhenBookingIsValid()
    {
        // Arrange
        var dto = new CreateBookingDto
        {
            MemberId = 1,
            GymClassId = 10
        };

        var gymClass = new GymClassEntity
        {
            Id = 10,
            Name = "Padel",
            Category = "Padel",
            Instructor = "David",
            StartTime = DateTime.UtcNow.AddDays(1),
            EndTime = DateTime.UtcNow.AddDays(1).AddHours(1),
            MaxParticipants = 10
        };

        _gymClassRepositoryMock
            .Setup(x => x.GetOneAsync(
                It.IsAny<Expression<Func<GymClassEntity, bool>>>(),
                false,
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<GymClassEntity, object>>[]>()))
            .ReturnsAsync(gymClass);

        _bookingRepositoryMock
            .Setup(x => x.ExistsAsync(
                It.IsAny<Expression<Func<BookingEntity, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _bookingService.CreateBookingAsync(dto);

        // Assert
        Assert.True(result.Succeeded);
        Assert.Null(result.ErrorMessage);

        _bookingRepositoryMock.Verify(x => x.AddAsync(
            It.Is<BookingEntity>(b =>
                b.MemberId == dto.MemberId &&
                b.GymClassId == dto.GymClassId),
            It.IsAny<CancellationToken>()), Times.Once);

        _bookingRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
