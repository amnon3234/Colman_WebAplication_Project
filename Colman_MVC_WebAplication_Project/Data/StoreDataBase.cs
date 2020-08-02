using System.Data.Entity;
using Colman_MVC_WebAplication_Project.Models;

namespace Colman_MVC_WebAplication_Project.Data
{
    public class StoreDataBase : DbContext
    {
        // Tables
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        // Many to many -> user's carts
        public DbSet<Cart> Carts { get; set; }

        // Many to many -> Order History 
        public DbSet<OrderHistory> OrderHistory { get; set; }

        // Create the many to many tables with 2 primary keys
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>().HasKey(p => new { p.UserID, p.ProductID });
            modelBuilder.Entity<OrderHistory>().HasKey(o => new { o.UserID, o.ProductID });
        }
    }
}