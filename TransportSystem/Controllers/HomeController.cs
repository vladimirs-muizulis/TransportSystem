using Microsoft.AspNetCore.Mvc;

namespace TransportSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
