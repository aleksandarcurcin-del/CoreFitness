using CoreFitness2.Application.Dtos.Classes;
using CoreFitness2.Application.Interfaces;
using CoreFitness2.Presentation.ViewModels.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness2.Presentation.Controllers;


[Authorize]
public class ClassesController(IGymClassService gymClassService) : Controller
{

    private readonly IGymClassService _gymClassService = gymClassService;

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var classes = await _gymClassService.GetAllAsync();
        var viewModel = new GymClassIndexViewModel
        {
            GymClass = classes
        };
        return View(viewModel);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateGymClassViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var dto = new CreateGymClassDto
        {
            Name = model.Name,
            Description = model.Description,
            Category = model.Category,
            Instructor = model.Instructor,
            StartTime = model.StartTime,
            EndTime = model.EndTime,
            MaxParticipants = model.MaxParticipants
        };

        var result = await _gymClassService.CreateAsync(dto);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Could not create class.");
            return View(model);
        }

        TempData["SuccessMessage"] = "Class created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _gymClassService.DeleteAsync(id);

        if (!result.Succeeded)
        {
            TempData["ErrorMessage"] = result.ErrorMessage ?? "Could not delete class.";
            return RedirectToAction(nameof(Index));
        }

        TempData["SuccessMessage"] = "Class deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}
