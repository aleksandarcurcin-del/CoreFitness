using CoreFitness2.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness2.Presentation.Controllers;

public class ClassesController(IGymClassService gymClassService) : Controller
{

    private readonly IGymClassService _gymClassService = gymClassService;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var classes = await _gymClassService.GetAllAsync();
        return View(classes);
    }
}
