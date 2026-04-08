// Controllers/CartController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WineShop.Data;
using Microsoft.AspNetCore.Http;
using WineShop.Models;

namespace WineShop.Controllers;


public class CartController : Controller
{
    private readonly ApplicationDbContext _db;

    public CartController(ApplicationDbContext db)
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

    // GET: /Cart
    public async Task<IActionResult> Index()
    {
        var sessionId = GetSessionId();
        var cartItems = await _db.CartItems
            .Include(c => c.Wine)
            .Where(c => c.SessionId == sessionId)
            .ToListAsync();

        return View(cartItems);
    }

    // POST: /Cart/UpdateQuantity
    [HttpPost]
    public async Task<IActionResult> UpdateQuantity([FromBody] UpdateCartRequest request)
    {
        var sessionId = GetSessionId();
        var cartItem = await _db.CartItems
            .FirstOrDefaultAsync(c => c.Id == request.CartItemId && c.SessionId == sessionId);

        if (cartItem == null)
            return Json(new { success = false, message = "Товар не найден в корзине" });

        if (request.NewQuantity <= 0)
        {
            _db.CartItems.Remove(cartItem);
        }
        else
        {
            cartItem.Quantity = request.NewQuantity;
        }

        await _db.SaveChangesAsync();

        // Пересчитываем общую сумму и количество
        var items = await _db.CartItems
            .Include(c => c.Wine)
            .Where(c => c.SessionId == sessionId)
            .ToListAsync();

        var totalItems = items.Sum(i => i.Quantity);
        var totalPrice = items.Sum(i => i.Quantity * (i.Wine?.Price ?? 0));

        return Json(new
        {
            success = true,
            newTotalItems = totalItems,
            newTotalPrice = totalPrice,
            itemSubtotal = (cartItem.Wine?.Price ?? 0) * (request.NewQuantity > 0 ? request.NewQuantity : 0)
        });
    }

    // POST: /Cart/RemoveItem
    [HttpPost]
    public async Task<IActionResult> RemoveItem([FromBody] RemoveCartRequest request)
    {
        var sessionId = GetSessionId();
        var cartItem = await _db.CartItems
            .FirstOrDefaultAsync(c => c.Id == request.CartItemId && c.SessionId == sessionId);

        if (cartItem != null)
        {
            _db.CartItems.Remove(cartItem);
            await _db.SaveChangesAsync();
        }

        return Json(new { success = true });
    }

    [HttpGet]
    public async Task<IActionResult> GetCartSummary()
    {
        var sessionId = GetSessionId();
        var items = await _db.CartItems
            .Include(c => c.Wine)
            .Where(c => c.SessionId == sessionId)
            .ToListAsync();

        var itemsTotal = items.Sum(i => i.Quantity * (i.Wine?.Price ?? 0));

        return Json(new { itemsTotal });
    }

    // Controllers/CartController.cs - добавьте эти методы

    // POST: /Cart/ClearCart
    [HttpPost]
    public async Task<IActionResult> ClearCart()
    {
        var sessionId = GetSessionId();
        var cartItems = await _db.CartItems
            .Where(c => c.SessionId == sessionId)
            .ToListAsync();

        _db.CartItems.RemoveRange(cartItems);
        await _db.SaveChangesAsync();

        return Json(new { success = true });
    }

    // POST: /Cart/ProcessOrder
    [HttpPost]
    public async Task<IActionResult> ProcessOrder([FromBody] OrderRequest request)
    {
        var sessionId = GetSessionId();
        var cartItems = await _db.CartItems
            .Include(c => c.Wine)
            .Where(c => c.SessionId == sessionId)
            .ToListAsync();

        if (!cartItems.Any())
        {
            return Json(new { success = false, message = "Корзина пуста" });
        }

        // Здесь можно сохранить заказ в базу данных
        // Создайте модель Order и OrderItem при необходимости

        // Рассчитываем сумму заказа
        var subtotal = cartItems.Sum(i => i.Quantity * (i.Wine?.Price ?? 0));
        var deliveryCost = 350;
        var total = subtotal + deliveryCost;

        // Логируем заказ (временно)
        Console.WriteLine($"=== НОВЫЙ ЗАКАЗ ===");
        Console.WriteLine($"Клиент: {request.CustomerName}");
        Console.WriteLine($"Телефон: {request.Phone}");
        Console.WriteLine($"Адрес: {request.Address}");
        Console.WriteLine($"Товары: {cartItems.Count} позиций");
        Console.WriteLine($"Сумма: {total} руб.");

        foreach (var item in cartItems)
        {
            Console.WriteLine($"  - {item.Wine?.Name} x {item.Quantity} = {(item.Wine?.Price ?? 0) * item.Quantity} руб.");
        }

        // Очищаем корзину после успешного оформления
        _db.CartItems.RemoveRange(cartItems);
        await _db.SaveChangesAsync();

        return Json(new { success = true, message = "Заказ успешно оформлен" });
    }



    // Модель для запроса заказа
    public class OrderRequest
    {
        public string Address { get; set; } = "";
        public string Phone { get; set; } = "";
        public string CustomerName { get; set; } = "";
    }

    public class UpdateCartRequest
    {
        public int CartItemId { get; set; }
        public int NewQuantity { get; set; }
    }

    public class RemoveCartRequest
    {
        public int CartItemId { get; set; }
    }
}
