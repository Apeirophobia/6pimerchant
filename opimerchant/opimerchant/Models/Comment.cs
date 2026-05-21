using System.ComponentModel.DataAnnotations;

namespace opimerchant.Models
{
    public class Comment
    {
        [Key]
        public Guid CommentID { get; set; }
        public Guid PostID { get; set; }
        public string Author { get; set; }
        [Required]
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
