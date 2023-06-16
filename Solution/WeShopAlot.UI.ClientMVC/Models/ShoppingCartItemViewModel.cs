using WeShopAlot.Data.Models;

namespace WeShopAlot.UI.ClientMVC.Models;

public class ShoppingCartItemViewModel
{
    public int Id { get; set; }
    public ProductViewModel? Product { get; set; }
    public int Qty { get; set; }
    public string? ShoppingCartId { get; set; }
}
