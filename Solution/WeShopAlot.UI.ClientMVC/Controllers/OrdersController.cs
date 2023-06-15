using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories.Interfaces;

namespace CoffeeShop.Controllers;

[Authorize]
public class OrdersController : Controller
{
    #region Member Variables

    private IBasketRepository basketRepository;

    #endregion Member Variables

    #region Constructors

    public OrdersController(IBasketRepository basketRepository)
    {
        this.basketRepository = basketRepository;
    }

    #endregion Construtors

    #region Public Actions

    public IActionResult Checkout()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Checkout(Order order)
    {
        //orderRepository.PlaceOrder(order);
        //basketRepository.ClearCart();
        HttpContext.Session.SetInt32("CartCount", 0);
        return RedirectToAction("CheckoutComplete");
    }
    
    public IActionResult CheckoutComplete()
    {
        return View();
    }

    #endregion Public Actions
}
