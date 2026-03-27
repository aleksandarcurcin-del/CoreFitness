using CoreFitness2.Infrastructure.Identity;
using CoreFitness2.Presentation.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness2.Presentation.Controllers;

public class AccountController : Controller
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;


    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }


    [HttpGet]
    public IActionResult Signup()
    {
        return View(new SignUpViewModel());
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Signup(SignUpViewModel model)
    {
        if (ModelState.IsValid)
            return View(model);

        return RedirectToAction(nameof(SetPassword), new {email = model.Email});
    }


    [HttpGet]
    public IActionResult SetPassword(string email)
    {
        var model = new SetPasswordViewModel { Email = email };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);

        return View(model);
    }
}
