using CoreFitness2.Application.Dtos.Auth;
using CoreFitness2.Application.Interfaces;
using CoreFitness2.Presentation.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness2.Presentation.Controllers;

public class AccountController : Controller
{
    private readonly IAuthService _authService;

    public AccountController(IAuthService authService)
    {
        _authService = authService;
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
        if (!ModelState.IsValid)
            return View(model);

        return RedirectToAction(nameof(SetPassword), new { email = model.Email });
    }

    [HttpGet]
    public IActionResult SetPassword(string email)
    {
        var model = new SetPasswordViewModel
        {
            Email = email
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var dto = new SignUpDto
        {
            Email = model.Email,
            Password = model.Password
        };

        var result = await _authService.RegisterAsync(dto);

        if (result.Succeeded)
            return RedirectToAction("Index", "Home");

        ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Registration failed.");
        return View(model);
    }

    [HttpGet]
    public IActionResult Signin()
    {
        return View(new SignInViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Signin(SignInViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var dto = new SignInDto
        {
            Email = model.Email,
            Password = model.Password
        };

        var result = await _authService.SignInAsync(dto);

        if (result.Succeeded)
            return RedirectToAction("Index", "Home");

        ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Invalid login attempt.");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Signout()
    {
        await _authService.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}