using Microsoft.AspNetCore.Mvc;

namespace opimerchant.Controllers
{
    public class SecretController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GrassBlock()
        {

            return View();
        }

        public IActionResult Cow()
        {

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> AccessDenied()
        {
            return View("AccessDenied");
        }
        
        [HttpPost]
        public async Task<IActionResult> AccessDeniedPost()
        {
            return RedirectToAction("Index", "Home");
        }

        
    }
}
