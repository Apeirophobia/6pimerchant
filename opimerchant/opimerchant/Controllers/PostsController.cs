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

            var comments = await _context.Comments
                .Where(c => c.PostID == id)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            ViewBag.Comments = comments;

            var commentIds = comments.Select(c => c.CommentID).ToList();
            var ratings = await _context.CommentRatings
                .Where(r => commentIds.Contains(r.CommentID))
                .ToListAsync();
            ViewBag.LikeCounts = ratings.Where(r => r.IsLike)
                .GroupBy(r => r.CommentID)
                .ToDictionary(g => g.Key, g => g.Count());

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> AccessDenied()
        {
            return View("AccessDenied", "Posts");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.PostID == id);
            if (post == null)
            {
                return NotFound();
            }
            if (!_signInManager.IsSignedIn(User) || post.Author != User.Identity?.Name)
            {
                return RedirectToAction("AccessDenied", "Posts");
            }
            return View(post);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.PostID == id);
            if (post == null)
            {
                return NotFound();
            }
            if (!_signInManager.IsSignedIn(User) || post.Author != User.Identity?.Name)
            {
                return RedirectToAction("AccessDenied", "Posts");
            }
            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, Post submitted)
        {
            var stored = await _context.Posts.FirstOrDefaultAsync(x => x.PostID == id);
            if (stored == null)
            {
                return NotFound();
            }
            if (!_signInManager.IsSignedIn(User) || stored.Author != User.Identity?.Name)
            {
                return RedirectToAction("AccessDenied", "Posts");
            }

            ModelState.Remove(nameof(Post.Author));
            if (ModelState.IsValid == true)
            {
                stored.Title = submitted.Title;
                stored.Description = submitted.Description;
                stored.Body = submitted.Body;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.PostID == id);
            if (post == null)
            {
                return NotFound();
            }
            if (!_signInManager.IsSignedIn(User) || post.Author != User.Identity?.Name)
            {
                return RedirectToAction("AccessDenied", "Posts");
            }
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
