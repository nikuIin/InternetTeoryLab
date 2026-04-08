using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WineShop.Models;

public class Wine
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Название обязательно")]
    [MinLength(3, ErrorMessage = "Минимум 3 символа")]
    [RegularExpression(@"^[\p{L}\s\-']+$", ErrorMessage = "Только буквы, пробелы, дефисы и апострофы")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Тип вина обязателен")]
    public string WineType { get; set; } = ""; // Красное, Белое, Розовое, Игристое, Десертное

    [Required(ErrorMessage = "Тип винограда обязателен")]
    public string GrapeType { get; set; } = "";

    [Range(1800, 2025, ErrorMessage = "Год должен быть от 1800 до 2025")]
    public int Year { get; set; }

    [Range(5.0, 25.0, ErrorMessage = "Алкоголь от 5.0 до 25.0")]
    [Column(TypeName = "decimal(4,1)")]
    public decimal Alcohol { get; set; }

    [Range(0, 999999, ErrorMessage = "Цена не может быть отрицательной")]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    [EmailAddress(ErrorMessage = "Некорректный email")]
    public string? ProducerEmail { get; set; }

    [Phone(ErrorMessage = "Некорректный телефон")]
    public string? ProducerPhone { get; set; }

    public string? ImageUrl { get; set; }

    public string? Description { get; set; }

    // Full-text search vector (stored as string, PostgreSQL uses tsvector)
    public string? SearchVector { get; set; }
}
