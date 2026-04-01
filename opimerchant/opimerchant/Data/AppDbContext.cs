using Microsoft.EntityFrameworkCore;
using opimerchant.Models;

namespace opimerchant.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        DbSet<User> Users { get; set; }
    }
}
