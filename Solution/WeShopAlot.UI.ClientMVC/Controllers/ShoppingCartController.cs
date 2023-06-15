using Microsoft.AspNetCore.Mvc;
using WeShopAlot.Data.Repositories.Interfaces;

namespace CoffeeShop.Controllers;

public class ShoppingCartController : Controller
{
    #region Member Variables

    private IProductRepository productRepository;
    private IBasketRepository basketRepository;

    #endregion Member Variables

    #region Constructors

    public ShoppingCartController(IBasketRepository basketRepository, IProductRepository productRepository)
    {
        this.basketRepository = basketRepository;
        this.productRepository = productRepository;
    }

    #endregion Constructors

    #region Public Actions

    public IActionResult Index()
    {
        var items = new List<object>();
        //var items = basketRepository.GetShoppingCartItems();
        //basketRepository.ShoppingCartItems = items;
        //ViewBag.CartTotal = basketRepository.GetShoppingCartTotal();
        return View(items);
    }

    public RedirectToActionResult AddToShoppingCart(int pId)
    {
        //var product = productRepository.GetAllProducts().FirstOrDefault(p => p.Id == pId);
        //if (product != null)
        //{
        //    basketRepository.AddToCart(product);
        //    int cartCount = basketRepository.GetShoppingCartItems().Count;
        //    HttpContext.Session.SetInt32("CartCount", cartCount);
        //}
        return RedirectToAction("Index");
    }

    public RedirectToActionResult RemoveFromShoppingCart(int pId)
    {
        //var product = productRepository.GetAllProducts().FirstOrDefault(p => p.Id == pId);
        //if (product != null)
        //{
        //    basketRepository.RemoveFromCart(product);
        //    int cartCount = basketRepository.GetShoppingCartItems().Count;
        //    HttpContext.Session.SetInt32("CartCount", cartCount);
        //}
        return RedirectToAction("Index");
    }

    #endregion Public Actions
}
