using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WineShop.Data;
using WineShop.Models;

namespace WineShop.Controllers;

// Controllers/WineController.cs - добавьте тот же метод-помощник

public class WineController : Controller
{
    private readonly ApplicationDbContext _db;

    public WineController(ApplicationDbContext db)
    {
        _db = db;
    }

    private string GetSessionId()
    {
        var sessionId = HttpContext.Session.GetString("CartSessionId");

        if (string.IsNullOrEmpty(sessionId))
        {
            sessionId = Guid.NewGuid().ToString();
            HttpContext.Session.SetString("CartSessionId", sessionId);

            Response.Cookies.Append("CartSessionId", sessionId, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(30),
                HttpOnly = true,
                SameSite = SameSiteMode.Lax
            });
        }

        return sessionId;
    }

    // GET /Wine/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var wine = await _db.Wines.FindAsync(id);
        if (wine == null) return NotFound();
        return View(wine);
    }

    // POST /Wine/AddToCart
    [HttpPost]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest req)
    {
        var wine = await _db.Wines.FindAsync(req.WineId);
        if (wine == null)
            return Json(new { success = false, message = "Вино не найдено" });

        var sessionId = GetSessionId();

        var existing = await _db.CartItems
            .FirstOrDefaultAsync(c => c.WineId == req.WineId && c.SessionId == sessionId);

        if (existing != null)
        {
            existing.Quantity += req.Quantity;
        }
        else
        {
            _db.CartItems.Add(new CartItem
            {
                WineId = req.WineId,
                Quantity = req.Quantity,
                SessionId = sessionId
            });
        }

        await _db.SaveChangesAsync();

        var cartCount = await _db.CartItems
            .Where(c => c.SessionId == sessionId)
            .SumAsync(c => c.Quantity);

        return Json(new
        {
            success = true,
            message = $"«{wine.Name}» добавлено в корзину",
            cartCount
        });
    }

    // GET /Wine/CartCount
    [HttpGet]
    public async Task<IActionResult> CartCount()
    {
        var sessionId = GetSessionId();
        var count = await _db.CartItems
            .Where(c => c.SessionId == sessionId)
            .SumAsync(c => (int?)c.Quantity) ?? 0;
        return Json(new { count });
    }
}

public class AddToCartRequest
{
    public int WineId { get; set; }
    public int Quantity { get; set; } = 1;
}
