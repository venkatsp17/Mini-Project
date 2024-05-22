using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Models;

namespace ShoppingAppAPI.Contexts
{
    public class ShoppingAppContext : DbContext
    {
        public ShoppingAppContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Refund> Refunds { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set;  }
        public DbSet<UserDetails> UserDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //User-UserDetails relationship
            modelBuilder.Entity<User>()
             .HasOne(u => u.UserDetails)
             .WithOne(ud => ud.User)
             .HasForeignKey<User>(u => u.UserID)
             .OnDelete(DeleteBehavior.Restrict);

            //Cart-User relationship

            modelBuilder.Entity<Cart>()
               .HasOne(c => c.User)
               .WithOne(u => u.Cart)
               .HasForeignKey<User>(c => c.UserID)
               .OnDelete(DeleteBehavior.Restrict);

            //Order-User relationship
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // Product-Category relationship
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            // Review-Product relationship
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductID)
                .OnDelete(DeleteBehavior.Restrict);

            // Review-User relationship
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // Order-OrderDetail relationship
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderID)
                .OnDelete(DeleteBehavior.Restrict);

            // OrderDetail-Product relationship
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductID)
                .OnDelete(DeleteBehavior.Restrict);


            // Cart-CartItem relationship
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartID)
                .OnDelete(DeleteBehavior.Restrict);

            // CartItem-Product relationship
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductID)
                .OnDelete(DeleteBehavior.Restrict);

            // Payment-Order relationship
            modelBuilder.Entity<Payment>()
                    .HasOne(p => p.Order)
                    .WithMany(o => o.Payments)
                    .HasForeignKey(p => p.OrderID)
                    .OnDelete(DeleteBehavior.Restrict);

            // Refund-Order relationship
            modelBuilder.Entity<Refund>()
                .HasOne(r => r.Order)
                .WithOne(o => o.Refund)
                .HasForeignKey<Refund>(r => r.OrderID)
                .OnDelete(DeleteBehavior.Restrict);
        }



    }
}
