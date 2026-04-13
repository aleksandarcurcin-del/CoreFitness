using Microsoft.AspNetCore.Mvc;

namespace CoreFitness2.Presentation.Controllers
{
    public class StoreController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
