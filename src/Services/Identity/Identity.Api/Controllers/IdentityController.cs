using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    public class IdentityController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
