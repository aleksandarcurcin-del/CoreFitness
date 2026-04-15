using Microsoft.AspNetCore.Mvc;

namespace CoreFitness2.Presentation.Controllers;

public class TrainingController : Controller
{
    public IActionResult PersonalTraining()
    {
        return View();
    }

    public IActionResult GroupTraining()
    {
        return View();
    }

    public IActionResult Padel()
    {
        return View();
    }
}
