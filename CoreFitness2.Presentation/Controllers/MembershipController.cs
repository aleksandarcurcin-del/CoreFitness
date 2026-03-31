using CoreFitness2.Application.Dtos.Memberships;
using CoreFitness2.Application.Interfaces;
using CoreFitness2.Presentation.ViewModels.Membership;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoreFitness2.Presentation.Controllers;

public class MembershipController : Controller
{
    private readonly IMembershipService _membershipService;

    public MembershipController(IMembershipService membershipService)
    {
        _membershipService = membershipService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var plans = await _membershipService.GetAllPlansAsync();

        UserMembershipDto? currentMembership = null;

        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrWhiteSpace(userId))
                currentMembership = await _membershipService.GetUserMembershipAsync(userId);
        }

        var model = new MembershipIndexViewModel
        {
            Plans = plans,
            CurrentMembership = currentMembership
        };

        return View(model);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Select(Guid membershipPlanId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
            return RedirectToAction("SignIn", "Account");

        var dto = new CreateMembershipDto
        {
            UserId = userId,
            MembershipPlanId = membershipPlanId
        };

        var success = await _membershipService.CreateMembershipAsync(dto);

        if (!success)
            TempData["MembershipError"] = "You already have a membership or the selected plan was not found.";

        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePlan(Guid membershipPlanId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
            return RedirectToAction("SignIn", "Account");

        var success = await _membershipService.ChangeMembershipPlanAsync(userId, membershipPlanId);

        if (!success)
            TempData["MembershipError"] = "Could not change membership plan.";

        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CancelMembership()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
            return RedirectToAction("SignIn", "Account");

        var success = await _membershipService.CancelMembershipAsync(userId);

        if (!success)
            TempData["MembershipError"] = "Could not cancel membership.";

        return RedirectToAction(nameof(Index));
    }
}