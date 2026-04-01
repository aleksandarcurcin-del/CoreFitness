using CoreFitness2.Application.Dtos.Bookings;
using CoreFitness2.Application.Interfaces;
using CoreFitness2.Infrastructure.Identity;
using CoreFitness2.Presentation.ViewModels.Bookings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness2.Presentation.Controllers;

[Authorize]
public class BookingsController(
    IBookingService bookingService,
    UserManager<ApplicationUser> userManager) : Controller
{
    private readonly IBookingService _bookingService = bookingService;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null)
            return Challenge();

        var bookings = await _bookingService.GetUserBookingsAsync(user.Id);

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
        var user = await _userManager.GetUserAsync(User);

        if (user is null)
            return Challenge();

        var result = await _bookingService.CreateBookingAsync(new CreateBookingDto
        {
            UserId = user.Id,
            GymClassId = gymClassId
        });

        if (!result.Succeeded)
        {
            TempData["BookingError"] = result.ErrorMessage;
        }
        else
        {
            TempData["BookingSuccess"] = "You have successfully booked the class!";
        }

        return RedirectToAction("Index", "Classes");
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int bookingId)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null)
            return Challenge();

        var result = await _bookingService.CancelBookingAsync(bookingId, user.Id);

        if (!result.Succeeded)
            TempData["BookingError"] = result.ErrorMessage;

        return RedirectToAction("Index");
    }
}