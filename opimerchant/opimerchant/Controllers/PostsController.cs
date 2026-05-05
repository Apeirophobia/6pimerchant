using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using opimerchant.Data;
using opimerchant.Models;

namespace opimerchant.Controllers
{
    public class PostsController : Controller
    {
        private readonly AppDbContext _context;
        public PostsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {

            var data = _context.Posts.ToList();
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            Post blank_post = new Post();
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
    }
}
