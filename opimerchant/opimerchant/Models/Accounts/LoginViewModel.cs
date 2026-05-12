using System.ComponentModel.DataAnnotations;
using Azure.Identity;

namespace opimerchant.Models.Accounts
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        
    }
}
