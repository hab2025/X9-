using HabCo.X9.Core;
using Microsoft.EntityFrameworkCore;

namespace HabCo.X9.Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Hall> Halls { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<InventoryItem> InventoryItems { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<KitchenOrder> KitchenOrders { get; set; }
    public DbSet<KitchenOrderItem> KitchenOrderItems { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<BookingService> BookingServices { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure the composite key for the BookingService join table
        modelBuilder.Entity<BookingService>()
            .HasKey(bs => new { bs.BookingId, bs.ServiceId });

        // Seed Roles
        var adminRole = new Role { Id = 1, Name = "Admin" };
        var receptionRole = new Role { Id = 2, Name = "Reception" };
        var kitchenRole = new Role { Id = 3, Name = "Kitchen" };
        modelBuilder.Entity<Role>().HasData(adminRole, receptionRole, kitchenRole);

        // Seed Admin User
        // Seed Admin User with a securely hashed password.
        // The password is 'admin'.
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = 1,
            Username = "admin",
            // Hashed using BCrypt.Net-Next
            PasswordHash = "$2a$11$GojGzD5d6Yffp8S4sA4jGuJkC/vjM2VwB/d2f9g.Z3vYlJ.L.Xq/S",
            RoleId = adminRole.Id
        });
    }
}