namespace WineShop.Models;

public class CartItem
{
    public int Id { get; set; }
    public int WineId { get; set; }
    public Wine? Wine { get; set; }
    public int Quantity { get; set; } = 1;
    public string SessionId { get; set; } = "";
}
