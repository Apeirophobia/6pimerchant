using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using opimerchant.Data;
using opimerchant.Models;
using opimerchant.Services;

namespace opimerchant.Controllers
{
    public class ProfilesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<opimerchant.Models.User> _signInManager;
        private readonly IFileUploadService _fileUploadService;
        public ProfilesController(
            AppDbContext context,
            SignInManager<opimerchant.Models.User> signInManager,
            IFileUploadService fileUploadService)
        {
            _context = context;
            _signInManager = signInManager;
            _fileUploadService = fileUploadService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            /*
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("AccessDenied", "Secret");
            }
            var loggedUser = await _signInManager.UserManager.GetUserAsync(User);
            ViewBag.loggedUser = loggedUser;
            return View();*/

            var data = _context.Users.ToList();

            return View(data);

        }
        [HttpGet]
        public async Task<IActionResult> MyProfile()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("AccessDenied", "Secret");
            }
            var loggedUser = await _signInManager.UserManager.GetUserAsync(User);
            ViewBag.loggedUser = loggedUser;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(string userName)
        {
            var profile = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);

            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var profile = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            
            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }

        [HttpPost]
        public async Task<IActionResult> EditConfirm(string id, User submitted, IFormFile? file)
        {
            var stored = await _context.Users.FirstOrDefaultAsync(x => x.Id == submitted.Id);

            if (stored == null)
            {
                return NotFound();
            }
            
            var fpath = await _fileUploadService.UploadFileAsync(file);

            if (ModelState.IsValid)
            {
                stored.Bio = submitted.Bio;
                if (fpath != "")
                {
                    stored.ProfilePicture = fpath;
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("MyProfile");
        }
    }
}
