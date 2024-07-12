
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RehberUygulamasi.Models;

namespace RehberUygulamasi.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet for Person entity
        public DbSet<Person> Persons { get; set; }
        public DbSet<Account> users { get; set; }
        public DbSet<Account> Accounts { get; set; }






    }
}

