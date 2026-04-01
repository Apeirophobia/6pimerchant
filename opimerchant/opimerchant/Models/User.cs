using System.ComponentModel.DataAnnotations;

namespace opimerchant.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        
    }
}
