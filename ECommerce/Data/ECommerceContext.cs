using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ECommerce.Models;

namespace ECommerce.Data
{
    public class ECommerceContext : DbContext
    {
        public ECommerceContext (DbContextOptions<ECommerceContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Orders> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<ECommerce.Models.Deal> Deal { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure entities, relationships, and other aspects of the database model here

            // Example: Configuring precision and scale for the Price property of the Product entity
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<ECommerce.Models.Testimonial> Testimonial { get; set; } = default!;

        
    }
}
