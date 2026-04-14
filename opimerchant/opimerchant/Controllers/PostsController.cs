using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
        {

            var data = _context.Posts.ToList();
            return View(data);
        }

        public IActionResult Create()
        {
            Post blank_post = new Post();
            return View("Create", blank_post);
        }

        public async Task<IActionResult> CreatePost(Post filled_post)
        {
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
    }
}
