using Microsoft.AspNetCore.Mvc;

namespace CoreFitness2.Presentation.Controllers;

public class FitnessCentersController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
