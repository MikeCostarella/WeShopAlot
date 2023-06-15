using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.Controllers
{
    public class ContactUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
