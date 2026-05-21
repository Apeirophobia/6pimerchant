using System.ComponentModel.DataAnnotations;

namespace opimerchant.Models
{
    public class CommentRating
    {
        [Key]
        public Guid CommentRatingID { get; set; }
        public Guid CommentID { get; set; }
        public string UserEmail { get; set; }
        public bool IsLike { get; set; }
    }
}
