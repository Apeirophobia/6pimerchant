using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using opimerchant.Data;
using opimerchant.Models;

namespace opimerchant.Controllers
{
    public class PostsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<opimerchant.Models.User> _signInManager;
        public PostsController(
            AppDbContext context,
            SignInManager<opimerchant.Models.User> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index()
        {

            var data = _context.Posts.ToList();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            Post blank_post = new Post();
            if (!_signInManager.IsSignedIn(User))
            {
                Console.WriteLine("How did you get here?");
                return RedirectToAction("AccessDenied", "Posts");
            }

            var loggedUser =  await _signInManager.UserManager.GetUserAsync(User);
            ViewBag.loggedUser = loggedUser;
            return View("Create", blank_post);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(Post filled_post)
        {
            // TODO: CHECK IF USER IS LOGGED IN 
            if (ModelState.IsValid == true)
            {
                var result = await _context.Posts.AddAsync(filled_post);
                if (result == null)
                {
                    return null;
                }
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _context.Posts.FirstOrDefaultAsync(x => x.PostID == id);

            if (result == null)
            {
                return NotFound();
            }
            
            return View(result);    
        }

        [HttpGet]
        public async Task<IActionResult> AccessDenied()
        {
            return View("AccessDenied", "Posts");
        }
    }
}
