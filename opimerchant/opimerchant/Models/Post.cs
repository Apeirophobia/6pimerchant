using System.ComponentModel.DataAnnotations;

namespace opimerchant.Models
{
    public class Post
    {
        [Key]
        public Guid ID { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string Body { get; set; }

    }
}
