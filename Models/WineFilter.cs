namespace WineShop.Models;

public class WineFilter
{
    public string? Search { get; set; }
    public string? WineType { get; set; }
    public string? GrapeType { get; set; }
    public int? YearFrom { get; set; }
    public int? YearTo { get; set; }
    public string SortBy { get; set; } = "default";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 16;
}
