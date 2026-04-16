using CoreFitness2.Application.Dtos.Auth;
using CoreFitness2.Application.Interfaces;
using CoreFitness2.Infrastructure.Identity;
using CoreFitness2.Presentation.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoreFitness2.Presentation.Controllers;

public class AccountController : Controller
{
    private readonly IAuthService _authService;
    private readonly ILogger<AccountController> _logger;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(

        IAuthService authService,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        ILogger<AccountController> logger)
    {
        _authService = authService;
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
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
        var schemes = await _signInManager.GetExternalAuthenticationSchemesAsync();
        var vm = new SignInViewModel
        {
            ReturnUrl = returnUrl,
            ExternalProviders = [.. schemes.Select(x => x.Name)]
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
        if (remoteError is not null)
        {
            _logger.LogWarning("Remote error from provider: {Error}", remoteError);
            return ExternalLoginFailed(returnUrl);
        }

        var ExternalUser = await GetExternalUserInfo();
        if (ExternalUser is null)
            return ExternalLoginFailed(returnUrl);
        
        var (info, email) = ExternalUser.Value;

        var result = await _signInManager.ExternalLoginSignInAsync
        (
            info.LoginProvider, 
            info.ProviderKey, 
            isPersistent: false, 
            bypassTwoFactor: true
        );


        if (result.Succeeded)
            return RedirectToLocal(returnUrl);


        return await ExternalVerification(email, returnUrl);


    }

    private async Task<IActionResult> ExternalVerification(string email, string? returnUrl = null)
    {

        // TODO: Generate code, save in database, send thru email, etc.

        return View("VerifyExternalLogin", new VerifyExternalLoginViewModel
        {
            ReturnUrl = returnUrl,
            Email = email,
        });    
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

        //TODO: validate code against database, etc.

        if(!string.Equals(vm.Code, "123456", StringComparison.Ordinal))
        {
            ModelState.AddModelError(nameof(vm.Code), "Invalid verification code.");
            return View("VerifyExternalLogin", vm);
        }

        var ExternalUser = await GetExternalUserInfo();
        if (ExternalUser is null)
            return ExternalLoginFailed(vm.ReturnUrl);

        var (info, email) = ExternalUser.Value;

        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser is not null)
            return await LinkExistingUser(existingUser, info, vm.ReturnUrl);

        return await CreateExternalUser(email, info, vm.ReturnUrl);

    }


    private async Task<IActionResult> LinkExistingUser(ApplicationUser user, ExternalLoginInfo info, string? returnUrl = null)
    {
        var result = await _userManager.AddLoginAsync(user, info);
        if (!result.Succeeded)
        {

            _logger.LogError("Failed to link {Provider} to {Email} : {Errors}",
                info.LoginProvider,
                user.Email, 
                string.Join(", ", result.Errors.Select(x => x.Description))
                );
            return ExternalLoginFailed(returnUrl);
        }

        await _signInManager.SignInAsync(user, isPersistent: false);
        return RedirectToLocal(returnUrl);
    }

    private async Task<IActionResult> CreateExternalUser(string email, ExternalLoginInfo info, string? returnUrl = null)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        var createResult = await _userManager.CreateAsync(user);
        if (!createResult.Succeeded)
        {
            _logger.LogError("Failed to create user {Email} : {Errors}",
                email,
                string.Join(", ", createResult.Errors.Select(x => x.Description))
                );
            return ExternalLoginFailed(returnUrl);
        }

        var LinkResult = await _userManager.AddLoginAsync(user, info);
        if (!LinkResult.Succeeded)
        {

            _logger.LogError("Failed to link {Provider} to {Email} : {Errors}",
                info.LoginProvider,
                user.Email,
                string.Join(", ", LinkResult.Errors.Select(x => x.Description))
                );
            return ExternalLoginFailed(returnUrl);
        }

        await _signInManager.SignInAsync(user, isPersistent: false);
        return RedirectToLocal(returnUrl);

    }


    private async Task<(ExternalLoginInfo Info, string Email)?> GetExternalUserInfo()
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info is null)
        {
            _logger.LogWarning("External login info is null.");
            return null;
        }


        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrWhiteSpace(email))
        {
            _logger.LogWarning("No email claim from {Provider}", info.LoginProvider);
            return null;
        }
        return (info, email);
    }


    private RedirectToActionResult ExternalLoginFailed(string? returnUrl = null)
    {
        TempData["ErrorMessage"] = "External login failed. Please try again.";
        return RedirectToAction(nameof(SignIn), new { returnUrl });
    }


    private IActionResult RedirectToLocal(string? returnUrl)
    {
        if(Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);


        return RedirectToAction("Index", "Home");



    }
    
    

}