using CoreFitness2.Application.Dtos.Bookings;
using CoreFitness2.Application.Interfaces;
using CoreFitness2.Presentation.ViewModels.Bookings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoreFitness2.Presentation.Controllers;

[Authorize]
public class BookingsController : Controller
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
            return Challenge();

        var bookings = await _bookingService.GetUserBookingsAsync(userId);

        var viewModel = new BookingIndexViewModel
        {
            Bookings = bookings
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Book(int gymClassId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
            return Challenge();

        var result = await _bookingService.CreateBookingAsync(new CreateBookingDto
        {
            UserId = userId,
            GymClassId = gymClassId
        });

        if (!result.Succeeded)
            TempData["BookingError"] = result.ErrorMessage;
        else
            TempData["BookingSuccess"] = "You have successfully booked the class!";

        return RedirectToAction("Index", "Classes");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int bookingId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
            return Challenge();

        var result = await _bookingService.CancelBookingAsync(bookingId, userId);

        if (!result.Succeeded)
            TempData["BookingError"] = result.ErrorMessage;

        return RedirectToAction(nameof(Index));
    }
}