using Microsoft.AspNetCore.Mvc;

namespace CoreFitness2.Presentation.Controllers;

public class ErrorController : Controller
{
    [Route("Error/{statusCode}")]
    public IActionResult ErrorHandler(int statusCode)
    {
        return statusCode switch
        {
            404 => View("NotFound"),
            _ => View()
        };
    }
}
