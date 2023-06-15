using Microsoft.AspNetCore.Mvc;
using WeShopAlot.Data.Repositories.Interfaces;

namespace CoffeeShop.Controllers;

public class ProductsController : Controller
{
    #region Member Variables

    private IProductRepository productRepository;

    #endregion Member Variables

    #region Constructors

    public ProductsController(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    #endregion Constructors

    #region Public Actions

    public IActionResult Shop()
    {
        return View(productRepository.GetProducts());
    }

    public IActionResult Detail(int id)
    {
        var product = productRepository.GetProductDetail(id);
        if(product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    #endregion Public Actions
}
