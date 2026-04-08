using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WineShop.Data;
using WineShop.Models;

namespace WineShop.Controllers;

public class CatalogController : Controller
{
    private readonly ApplicationDbContext _db;

    public CatalogController(ApplicationDbContext db)
    {
        _db = db;
    }

    // GET: /Catalog
    public IActionResult Index()
    {
        return View();
    }

    // GET: /Catalog/GetWines
    [HttpGet]
    public async Task<IActionResult> GetWines([FromQuery] WineFilter filter)
    {
        var query = _db.Wines.AsQueryable();

        // Полнотекстовый поиск
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var s = filter.Search.ToLower();
            query = query.Where(w =>
                w.Name.ToLower().Contains(s) ||
                w.WineType.ToLower().Contains(s) ||
                w.GrapeType.ToLower().Contains(s) ||
                (w.Description != null && w.Description.ToLower().Contains(s)));
        }

        // Фильтр по типу вина
        if (!string.IsNullOrWhiteSpace(filter.WineType))
            query = query.Where(w => w.WineType == filter.WineType);

        // Фильтр по типу винограда
        if (!string.IsNullOrWhiteSpace(filter.GrapeType))
            query = query.Where(w => w.GrapeType == filter.GrapeType);

        // Фильтр по году
        if (filter.YearFrom.HasValue)
            query = query.Where(w => w.Year >= filter.YearFrom.Value);
        if (filter.YearTo.HasValue)
            query = query.Where(w => w.Year <= filter.YearTo.Value);

        // Сортировка
        query = filter.SortBy switch
        {
            "price-asc"  => query.OrderBy(w => w.Price),
            "price-desc" => query.OrderByDescending(w => w.Price),
            "name-asc"   => query.OrderBy(w => w.Name),
            "name-desc"  => query.OrderByDescending(w => w.Name),
            _            => query.OrderBy(w => w.Id)
        };

        var total = await query.CountAsync();

        // Пагинация
        var wines = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(w => new
            {
                w.Id,
                w.Name,
                w.WineType,
                w.GrapeType,
                w.Year,
                w.Price,
                w.Alcohol,
                w.ImageUrl
            })
            .ToListAsync();

        return Json(new
        {
            items = wines,
            total,
            page = filter.Page,
            pageSize = filter.PageSize,
            totalPages = (int)Math.Ceiling((double)total / filter.PageSize)
        });
    }

    // GET: /Catalog/GetFilterOptions
    [HttpGet]
    public async Task<IActionResult> GetFilterOptions()
    {
        var wineTypes  = await _db.Wines.Select(w => w.WineType).Distinct().OrderBy(x => x).ToListAsync();
        var grapeTypes = await _db.Wines.Select(w => w.GrapeType).Distinct().OrderBy(x => x).ToListAsync();
        var minYear    = await _db.Wines.MinAsync(w => (int?)w.Year) ?? 2000;
        var maxYear    = await _db.Wines.MaxAsync(w => (int?)w.Year) ?? DateTime.Now.Year;

        return Json(new { wineTypes, grapeTypes, minYear, maxYear });
    }

    [HttpGet]
    public async Task<IActionResult> SearchSuggestions(string term)
    {
        if (string.IsNullOrWhiteSpace(term) || term.Length < 2)
        {
            return Json(new List<object>());
        }

        var suggestions = await _db.Wines
            .Where(w => w.Name.ToLower().Contains(term.ToLower()))
            .OrderBy(w => w.Name)
            .Take(5) // Показываем не более 5 подсказок
            .Select(w => new { w.Id, w.Name })
            .ToListAsync();

        return Json(suggestions);
    }
}
