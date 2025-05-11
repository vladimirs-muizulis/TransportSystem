using Microsoft.AspNetCore.Mvc;

namespace TransportSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Check if the session variable "Username" exists
            var username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
            {
                // If "Username" is not found in the session, redirect to the login page
                return RedirectToAction("Login", "Account");
            }

            // Get the user's role from the session
            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;  // Pass the role to the View

            // If the user is authenticated, return the view from the "User" folder
            return View("~/Views/User/TransportList.cshtml");  // Correct path to the view
        }
    }
}

