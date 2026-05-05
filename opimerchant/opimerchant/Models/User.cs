using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace opimerchant.Models
{
    public class User : IdentityUser
    {
        public List<string>? Orders { get; set; }
        
    }
}
