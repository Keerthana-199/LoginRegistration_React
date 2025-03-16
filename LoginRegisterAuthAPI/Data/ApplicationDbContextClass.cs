using Microsoft.EntityFrameworkCore;
using LoginRegisterAuthAPI.Models;
using System.Collections.Generic;
using System.Reflection.Emit;
namespace LoginRegisterAuthAPI.Data
{
    public class ApplicationDbContextClass : DbContext
    {
        public ApplicationDbContextClass(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Users> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map stored procedures
            modelBuilder.Entity<Users>().HasNoKey(); // Required for SP results that don't map directly
        }
    }

}
