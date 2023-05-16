using WeShopAlot.WebAPI.Errors;
using Microsoft.AspNetCore.Mvc;

namespace WeShopAlot.WebAPI.Controllers
{
    [Route("errors/{code}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : BaseApiController
    {
        public IActionResult Error(int code)
        {
            return new ObjectResult(new ApiResponse(code));
        }
    }
}
