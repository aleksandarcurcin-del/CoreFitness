using CoreFitness2.Application.Dtos.Auth;
using CoreFitness2.Application.Interfaces;
using CoreFitness2.Application.Results;
using CoreFitness2.Infrastructure.Identity;
using CoreFitness2.Presentation.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness2.Presentation.Controllers;

public class AccountController : Controller
{
    private readonly IAuthService _authService;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(

        IAuthService authService,
        SignInManager<ApplicationUser> signInManager) 
    {
        _authService = authService;
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
    public async Task<IActionResult> SignIn(string? returnUrl = null)
    {
        var providers = await _authService.GetExternalProvidersAsync();

        var vm = new SignInViewModel
        {
            ReturnUrl = returnUrl,
            ExternalProviders = [.. providers]
        };

        return View(vm);
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ExternalLogin(string provider, string? returnUrl = null)
    {
        if (string.IsNullOrWhiteSpace(provider))
            return RedirectToAction(nameof(SignIn), new { returnUrl });

        var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
    }



    [HttpGet]
    public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
    {
        var result = await _authService.HandleExternalLoginCallbackAsync(returnUrl, remoteError);

        if (result.Type == AuthenticationResultType.SignedIn)
            return RedirectToLocal(result.ReturnUrl);

        if (result.Type == AuthenticationResultType.RequiresVerification)
        {
            return View("VerifyExternalLogin", new VerifyExternalLoginViewModel
            {
                Email = result.Email!,
                ReturnUrl = result.ReturnUrl
            });
        }

        TempData["ErrorMessage"] = "External login failed.";
        return RedirectToAction(nameof(SignIn), new { returnUrl });
    }



#if DEBUG
    [HttpGet]
    public IActionResult TestVerifyExternalLogin()
    {
        return View("VerifyExternalLogin", new VerifyExternalLoginViewModel
        {
            Email = "test@domain.com",
            ReturnUrl = "/"
        });
    }


#endif


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> VerifyExternalLogin(VerifyExternalLoginViewModel vm)
    {
        if (!ModelState.IsValid)
            return View("VerifyExternalLogin", vm);

        var result = await _authService.VerifyExternalLoginAsync(vm.Code, vm.ReturnUrl);

        if (result.Type == AuthenticationResultType.SignedIn)
            return RedirectToLocal(result.ReturnUrl);

        if (result.Type == AuthenticationResultType.RequiresVerification)
        {
            return View("VerifyExternalLogin", new VerifyExternalLoginViewModel
            {
                Email = result.Email!,
                ReturnUrl = result.ReturnUrl
            });
        }

        if (result.Type == AuthenticationResultType.InvalidCode)
        {
            ModelState.AddModelError(nameof(vm.Code), "Invalid verification code.");
            return View("VerifyExternalLogin", vm);
        }

        ModelState.AddModelError(nameof(vm.Code), "External login failed.");
        return View("VerifyExternalLogin", vm);
    }


    private IActionResult RedirectToLocal(string? returnUrl)
    {
        if(Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);


        return RedirectToAction("Index", "Home");



    }
    
    

}