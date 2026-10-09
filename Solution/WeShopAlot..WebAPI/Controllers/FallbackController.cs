using Microsoft.AspNetCore.Mvc;

namespace WeShopAlot.WebAPI.Controllers
{
    public class FallbackController : Controller
    {
        private readonly IWebHostEnvironment environment;

        public FallbackController(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        // Serves the Angular app for any route the API does not handle.
        public IActionResult Index()
        {
            return PhysicalFile(Path.Combine(environment.ContentRootPath, "wwwroot", "index.html"), "text/HTML");
        }
    }
}
