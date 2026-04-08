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
    private readonly IMemberService _memberService;

    public BookingsController(IBookingService bookingService, IMemberService memberService)
    {
        _bookingService = bookingService;
        _memberService = memberService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var applicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(applicationUserId))
            return Challenge();

        var member = await _memberService.GetByApplicationUserIdAsync(applicationUserId);

        if (member is null)
            return Challenge();

        var bookings = await _bookingService.GetMemberBookingsAsync(member.Id);

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
        var applicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(applicationUserId))
            return Challenge();

        var member = await _memberService.GetByApplicationUserIdAsync(applicationUserId);

        if (member is null)
            return Challenge();

        var result = await _bookingService.CreateBookingAsync(new CreateBookingDto
        {
            MemberId = member.Id,
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
        var applicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(applicationUserId))
            return Challenge();

        var member = await _memberService.GetByApplicationUserIdAsync(applicationUserId);

        if (member is null)
            return Challenge();

        var result = await _bookingService.CancelBookingAsync(bookingId, member.Id);

        if (!result.Succeeded)
            TempData["BookingError"] = result.ErrorMessage;

        return RedirectToAction(nameof(Index));
    }
}