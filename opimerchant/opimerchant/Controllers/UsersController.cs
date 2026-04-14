using Microsoft.AspNetCore.Mvc;

namespace opimerchant.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
