using Microsoft.AspNetCore.Mvc;
using WeShopAlot.UI.ClientMVC.Services;

namespace CoffeeShop.Controllers;

public class ProductsController : Controller
{
    #region Member Variables

    private IProductService productService;

    #endregion Member Variables

    #region Constructors

    public ProductsController(IProductService productService)
    {
        this.productService = productService;
    }

    #endregion Constructors

    #region Public Actions

    public IActionResult Shop()
    {
        return View(productService.GetProducts());
    }

    public IActionResult Detail(int id)
    {
        var product = productService.GetProductDetail(id);
        if(product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    #endregion Public Actions
}
