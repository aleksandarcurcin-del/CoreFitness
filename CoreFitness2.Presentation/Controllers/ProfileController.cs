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
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProfileController(IMemberService memberService, IWebHostEnvironment webHostEnvironment)
    {
        _memberService = memberService;
        _webHostEnvironment = webHostEnvironment;
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
            PhoneNumber = member.PhoneNumber,
            ExistingProfileImageUrl = member.ProfileImageUrl
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

        var member = await _memberService.GetByApplicationUserIdAsync(applicationUserId);

        if (member == null)
            return RedirectToAction("Signin", "Account");

        string? profileImageUrl = member.ProfileImageUrl;

        if (model.ProfileImage != null && model.ProfileImage.Length > 0)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "profiles");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileExtension = Path.GetExtension(model.ProfileImage.FileName);
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await model.ProfileImage.CopyToAsync(stream);

            profileImageUrl = $"/images/profiles/{fileName}";
        }

        var dto = new UpdateMemberDto
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            ProfileImageUrl = profileImageUrl
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