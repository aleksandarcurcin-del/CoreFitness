using CoreFitness2.Application.Dtos.Members;
using CoreFitness2.Application.Interfaces;
using CoreFitness2.Presentation.ViewModels.Profile;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoreFitness2.Presentation.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly IMemberService _memberService;

    public ProfileController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var applicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(applicationUserId))
            return RedirectToAction("Signin", "Account");

        var member = await _memberService.GetByApplicationUserIdAsync(applicationUserId);

        if (member == null)
            return RedirectToAction("Signin", "Account");

        var model = new ProfileViewModel
        {
            FirstName = member.FirstName,
            LastName = member.LastName,
            Email = member.Email,
            PhoneNumber = member.PhoneNumber
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ProfileViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var applicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(applicationUserId))
            return RedirectToAction("Signin", "Account");

        var dto = new UpdateMemberDto
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber
        };

        var result = await _memberService.UpdateAsync(applicationUserId, dto);

        if (result.Succeeded)
        {
            TempData["SuccessMessage"] = "Profile updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Could not update profile.");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAccount()
    {
        var applicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(applicationUserId))
            return RedirectToAction("Signin", "Account");

        var result = await _memberService.DeleteAsync(applicationUserId);

        if (!result.Succeeded)
        {
            TempData["ErrorMessage"] = result.ErrorMessage ?? "Could not delete account.";
            return RedirectToAction(nameof(Index));
        }

        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        return RedirectToAction("Index", "Home");
    }
}