using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TransportSystem.Data; 
using TransportSystem.Models; 
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AdminController : Controller
{
    private readonly AppDbContext _dbContext;

    public AdminController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

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