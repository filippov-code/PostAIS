using Microsoft.EntityFrameworkCore;
using PostAIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostAIS.Database
{
    public class PostAisDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<SendPackageService> SendPackageServices { get; set; }
        public DbSet<ReceivePackageService> ReceivePackageServices { get; set; }
        public DbSet<ProductPurchaseService> ProductPurchaseServices { get; set; }
        public DbSet<PackageToReceive> PackagesToReceive { get; set; }
        public DbSet<RegistrationCode> RegistrationCodes { get; set; }

        public PostAisDbContext()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-J4O7180\SQLEXPRESS;Initial Catalog = PostAisDb;Integrated Security=True;Encrypt=False;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

    }
}
