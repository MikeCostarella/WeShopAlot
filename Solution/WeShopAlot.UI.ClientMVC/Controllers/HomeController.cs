using Microsoft.AspNetCore.Mvc;
using WeShopAlot.UI.ClientMVC.Services;

namespace CoffeeShop.Controllers;

public class HomeController : Controller
{
    #region Member Variables

    private IProductService productService;

    #endregion Member Variables

    #region #Constructors

    public HomeController(IProductService productService)
    {
        this.productService = productService; 
    }

    #endregion Constructors

    #region Public Actions

    public IActionResult Index()
    {
        return View(productService.GetTrendingProducts());
    }

    #endregion Constructors
}
