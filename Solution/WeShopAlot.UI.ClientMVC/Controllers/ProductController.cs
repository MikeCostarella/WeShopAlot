using Microsoft.AspNetCore.Mvc;
using WeShopAlot.UI.ClientMVC.Models;

namespace WeShopAlot.UI.ClientMVC.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            var products = new List<ProductViewModel> {
                new ProductViewModel {
                    Description = "Description for Product 1",
                    Name = "Product 1",
                    Price = 100
                },
                new ProductViewModel
                {
                    Description = "Description for Product 1",
                    Name = "Product 2",
                    Price = 200
                }            
            };
            ViewBag.Products = products;
            return View();
        }

        [HttpPost]
        public IActionResult Details(ProductViewModel product)
        {
            return RedirectToAction("Index");
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            return RedirectToAction("Index");
        }

    }
}
