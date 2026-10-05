using Microsoft.EntityFrameworkCore;
using PizzaStoreApi.Models;

namespace PizzaStoreApi.Data
{
    public class PizzaDbContext : DbContext
    {
        public PizzaDbContext(DbContextOptions<PizzaDbContext> options) : base(options) { }

        public DbSet<Pizza> Pizzas => Set<Pizza>();
        public DbSet<Topping> Toppings => Set<Topping>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Enforce unique topping names to prevent duplicates[cite: 1]
            modelBuilder.Entity<Topping>()
                .HasIndex(t => t.Name)
                .IsUnique();

            // Enforce unique pizza names to prevent duplicates[cite: 1]
            modelBuilder.Entity<Pizza>()
                .HasIndex(p => p.Name)
                .IsUnique();

            // Let EF Core handle the many-to-many relationship automatically
            modelBuilder.Entity<Pizza>()
                .HasMany(p => p.Toppings)
                .WithMany(t => t.Pizzas);
        }
    }
}