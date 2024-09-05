using Microsoft.EntityFrameworkCore;
using Wepapi_Management.Model;

namespace Wepapi_Management
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }


        public DbSet<ApplicationUser> Application { get; set; }
    }
}
