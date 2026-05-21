using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using opimerchant.Data;
using opimerchant.Models;

namespace opimerchant.Controllers
{
    public class CommentsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<User> _signInManager;

        public CommentsController(
            AppDbContext context,
            SignInManager<User> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> CommentPost(Guid postId, string body)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("AccessDenied", "Posts");
            }
            if (string.IsNullOrWhiteSpace(body))
            {
                return RedirectToAction("Details", "Posts", new { id = postId });
            }

            var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostID == postId);
            if (post == null)
            {
                return NotFound();
            }

            var comment = new Comment
            {
                CommentID = Guid.NewGuid(),
                PostID = postId,
                Author = User.Identity?.Name,
                Body = body,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Posts", new { id = postId });
        }

        [HttpPost]
        public async Task<IActionResult> Like(Guid commentId)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("AccessDenied", "Posts");
            }

            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.CommentID == commentId);
            if (comment == null)
            {
                return NotFound();
            }

            var userEmail = User.Identity?.Name;
            var existing = await _context.CommentRatings
                .FirstOrDefaultAsync(r => r.CommentID == commentId && r.UserEmail == userEmail);

            if (existing == null)
            {
                await _context.CommentRatings.AddAsync(new CommentRating
                {
                    CommentRatingID = Guid.NewGuid(),
                    CommentID = commentId,
                    UserEmail = userEmail,
                    IsLike = true
                });
            }
            else if (existing.IsLike)
            {
                _context.CommentRatings.Remove(existing);
            }
            else
            {
                existing.IsLike = true;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Posts", new { id = comment.PostID });
        }

        [HttpPost]
        public async Task<IActionResult> Dislike(Guid commentId)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("AccessDenied", "Posts");
            }

            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.CommentID == commentId);
            if (comment == null)
            {
                return NotFound();
            }

            var userEmail = User.Identity?.Name;
            var existing = await _context.CommentRatings
                .FirstOrDefaultAsync(r => r.CommentID == commentId && r.UserEmail == userEmail);

            if (existing == null)
            {
                await _context.CommentRatings.AddAsync(new CommentRating
                {
                    CommentRatingID = Guid.NewGuid(),
                    CommentID = commentId,
                    UserEmail = userEmail,
                    IsLike = false
                });
            }
            else if (!existing.IsLike)
            {
                _context.CommentRatings.Remove(existing);
            }
            else
            {
                existing.IsLike = false;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Posts", new { id = comment.PostID });
        }
    }
}
