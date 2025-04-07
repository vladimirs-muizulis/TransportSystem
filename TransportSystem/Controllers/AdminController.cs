using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

public class AdminController : Controller
{
    public IActionResult Dashboard()
    {
        var role = HttpContext.Session.GetString("Role");
        if (HttpContext.Session.GetString("Username") == null || role != "Admin")
        {
            return RedirectToAction("Login", "Account");
        }

        return View();
    }
}