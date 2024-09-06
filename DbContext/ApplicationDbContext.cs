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
        public DbSet<user> Users { get; set; }
        public DbSet<role> roles { get; set; }
        public DbSet<userRole> userRoles { get; set; }
        public DbSet<Employee> employees { get; set; }
    }
}
