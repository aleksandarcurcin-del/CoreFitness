using CoreFitness2.Infrastructure.Identity;
using CoreFitness2.Presentation.ViewModels.Profile;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness2.Presentation.Controllers;


[Authorize]
public class ProfileController : Controller
{

    private readonly UserManager<ApplicationUser> _userManager;
    public ProfileController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return RedirectToAction("Signin", "Myaccount");


        var model = new ProfileViewModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email ?? null!,
            PhoneNumber = user.PhoneNumber
        };

        return View(model);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ProfileViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);


        var user = await _userManager.GetUserAsync(User);


        if (user == null)
            return RedirectToAction("Signin", "Myaccount");


        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Email = model.Email;
        user.PhoneNumber = model.PhoneNumber;


        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            TempData["SuccessMessage"] = "Profile updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);

        return View(model);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAccount()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return RedirectToAction("Signin", "Myaccount");

        await _userManager.DeleteAsync(user);
        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

        return RedirectToAction("Index", "Home");
    }
}
