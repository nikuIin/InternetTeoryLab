using System.ComponentModel.DataAnnotations;

namespace WineShop.Models;

public class AddWineViewModel
{
    [Required(ErrorMessage = "Название обязательно")]
    [MinLength(3, ErrorMessage = "Минимум 3 символа")]
    [RegularExpression(@"^[\p{L}\s\-']+$", ErrorMessage = "Только буквы, пробелы, дефисы и апострофы")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Тип вина обязателен")]
    public string WineType { get; set; } = "";

    [Required(ErrorMessage = "Тип винограда обязателен")]
    public string GrapeType { get; set; } = "";

    [Required(ErrorMessage = "Год обязателен")]
    [Range(1800, 2025, ErrorMessage = "Год должен быть от 1800 до 2025")]
    public int Year { get; set; }

    [Required(ErrorMessage = "Алкоголь обязателен")]
    [Range(5.0, 25.0, ErrorMessage = "Алкоголь от 5.0 до 25.0")]
    public decimal Alcohol { get; set; }

    [Required(ErrorMessage = "Цена обязательна")]
    [Range(0, 999999, ErrorMessage = "Цена не может быть отрицательной")]
    public decimal Price { get; set; }

    [EmailAddress(ErrorMessage = "Некорректный email")]
    public string? ProducerEmail { get; set; }

    [RegularExpression(@"^\+7 \(\d{3}\) \d{3}-\d{2}-\d{2}$", ErrorMessage = "Формат: +7 (XXX) XXX-XX-XX")]
    public string? ProducerPhone { get; set; }

    public string? Description { get; set; }

    public IFormFile? Image { get; set; }
}
