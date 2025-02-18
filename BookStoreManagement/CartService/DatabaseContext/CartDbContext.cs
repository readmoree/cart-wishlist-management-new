using Microsoft.EntityFrameworkCore;
using CartService.Entity;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CartService.DatabaseContext
{
    public class CartDbContext : DbContext
    {
        public CartDbContext(DbContextOptions<CartDbContext> options) : base(options) { }
        public DbSet<Cart> Carts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>().HasKey(c => new { c.CustomerId, c.BookId });
            base.OnModelCreating(modelBuilder);
        }
    }
}
