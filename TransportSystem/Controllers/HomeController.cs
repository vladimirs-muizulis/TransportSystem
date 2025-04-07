using Microsoft.AspNetCore.Mvc;

namespace TransportSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Проверка, существует ли сессионная переменная "Username"
            var username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
            {
                // Если переменная "Username" отсутствует в сессии, перенаправляем на страницу входа
                return RedirectToAction("Login", "Account");
            }

            // Получаем роль пользователя из сессии
            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;  // Передаем роль в View

            // Если пользователь авторизован, возвращаем представление из папки "User"
            return View("~/Views/User/TransportList.cshtml");  // Правильный путь к представлению
        }
    }
}
