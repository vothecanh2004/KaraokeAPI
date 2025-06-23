using Microsoft.EntityFrameworkCore;
using KaraokeAPI.Models;

namespace KaraokeAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<FoodOrder> FoodOrders { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // USER
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

            // ROOM
            modelBuilder.Entity<Room>(entity =>
            {
                entity.Property(r => r.PricePerHour).HasPrecision(18, 2);
            });

            // BOOKING → ROOM (N-1)
            modelBuilder.Entity<Booking>()
    .HasOne(b => b.Room)
    .WithMany(r => r.Bookings)
    .HasForeignKey(b => b.RoomId)
    .OnDelete(DeleteBehavior.Restrict);

            // BILL → BOOKING (1-1)
            modelBuilder.Entity<Bill>(entity =>
            {
                entity.Property(b => b.TotalAmount).HasPrecision(18, 2);

                entity.HasOne(b => b.Booking)
                      .WithOne()
                      .HasForeignKey<Bill>(b => b.BookingId)
                      .OnDelete(DeleteBehavior.Cascade); // OK vì không tạo vòng

                entity.HasOne(b => b.Room)
                      .WithMany()
                      .HasForeignKey(b => b.RoomId)
                      .OnDelete(DeleteBehavior.Restrict); // ✅ TRÁNH xung đột cascade
            });

            // FOOD
            modelBuilder.Entity<Food>(entity =>
            {
                entity.Property(f => f.Price).HasPrecision(18, 2);
            });
        }
    }
}
