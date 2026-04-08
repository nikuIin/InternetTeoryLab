using Microsoft.AspNetCore.Mvc;
using WineShop.Data;
using WineShop.Models;

namespace WineShop.Controllers;

public class AddWineController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly IWebHostEnvironment _env;

    public AddWineController(ApplicationDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    // GET /AddWine
    public IActionResult Index()
    {
        return View();
    }

    // POST /AddWine — AJAX: добавить вино (ЛР 5)
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] AddWineViewModel model)
    {
        // Серверная валидация ModelState (DataAnnotations)
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                );
            return Json(new { success = false, errors });
        }

        // Дополнительная серверная проверка, которую нельзя сделать на клиенте:
        // уникальность названия (не проверяется клиентом)
        var nameExists = _db.Wines.Any(w => w.Name.ToLower() == model.Name.ToLower());
        if (nameExists)
        {
            return Json(new
            {
                success = false,
                errors = new Dictionary<string, string[]>
                {
                    { "Name", new[] { "Вино с таким названием уже существует в каталоге" } }
                }
            });
        }

        // Сохранение изображения
        string? imageUrl = null;
        if (model.Image != null && model.Image.Length > 0)
        {
            // Проверка типа файла на сервере (клиент уже проверил, но повторяем)
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
            if (!allowedTypes.Contains(model.Image.ContentType))
            {
                return Json(new
                {
                    success = false,
                    errors = new Dictionary<string, string[]>
                    {
                        { "Image", new[] { "Допустимые форматы: JPG, PNG, GIF, WEBP" } }
                    }
                });
            }

            // Проверка размера (макс 2MB)
            if (model.Image.Length > 2 * 1024 * 1024)
            {
                return Json(new
                {
                    success = false,
                    errors = new Dictionary<string, string[]>
                    {
                        { "Image", new[] { "Размер файла не должен превышать 2MB" } }
                    }
                });
            }

            var uploadsDir = Path.Combine(_env.WebRootPath, "img", "wine-products", "uploads");
            Directory.CreateDirectory(uploadsDir);

            var ext = Path.GetExtension(model.Image.FileName);
            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsDir, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await model.Image.CopyToAsync(stream);

            imageUrl = $"/img/wine-products/uploads/{fileName}";
        }

        var wine = new Wine
        {
            Name         = model.Name.Trim(),
            WineType     = model.WineType,
            GrapeType    = model.GrapeType,
            Year         = model.Year,
            Alcohol      = model.Alcohol,
            Price        = model.Price,
            ProducerEmail = model.ProducerEmail,
            ProducerPhone = model.ProducerPhone,
            Description  = model.Description,
            ImageUrl     = imageUrl ?? "/img/wine-products/wine-product1.webp"
        };

        _db.Wines.Add(wine);
        await _db.SaveChangesAsync();

        return Json(new
        {
            success = true,
            message = $"Вино «{wine.Name}» успешно добавлено в каталог",
            wineId = wine.Id
        });
    }
}
