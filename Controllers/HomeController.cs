using Microsoft.AspNetCore.Mvc;

namespace WineShop.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Error()
    {
        return Problem("Произошла ошибка при обработке запроса");
    }
}
