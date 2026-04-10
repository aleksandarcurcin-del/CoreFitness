using CoreFitness2.Presentation.ViewModels.Support;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness2.Presentation.Controllers;

public class SupportController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View(new CustomerServiceViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(CustomerServiceViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Please correct the form and try again.";
            return View(model);
        }

        TempData["SuccessMessage"] = "Your message has been sent successfully.";
        return RedirectToAction(nameof(Index));
    }
}