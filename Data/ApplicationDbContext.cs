using Microsoft.EntityFrameworkCore;
using KaraokeAPI.Models;

namespace KaraokeAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<FoodOrder> FoodOrders { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.FullName).HasMaxLength(100);
                entity.Property(e => e.Role).HasDefaultValue("User");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });

            // ✅ Thêm cấu hình cho Room entity
            modelBuilder.Entity<Room>(entity =>
            {
                // Cấu hình độ chính xác cho PricePerHour
                entity.Property(r => r.PricePerHour)
                      .HasPrecision(18, 2);
            });
            modelBuilder.Entity<Bill>(entity =>
            {
                entity.Property(b => b.TotalAmount)
                    .HasPrecision(18, 2);
    });
            modelBuilder.Entity<Food>(entity =>
{
    entity.Property(f => f.Price)
          .HasPrecision(18, 2);
});
        }
    }
}
