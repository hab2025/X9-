using HabCo.X9.Core;
using Microsoft.EntityFrameworkCore;

namespace HabCo.X9.Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Hall> Halls { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed Roles
        var adminRole = new Role { Id = 1, Name = "Admin" };
        var receptionRole = new Role { Id = 2, Name = "Reception" };
        var kitchenRole = new Role { Id = 3, Name = "Kitchen" };
        modelBuilder.Entity<Role>().HasData(adminRole, receptionRole, kitchenRole);

        // Seed Admin User
        // In a real application, never store plain text passwords.
        // This is a placeholder and should be replaced with a secure hashing algorithm.
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = 1,
            Username = "admin",
            // NOTE: This is NOT a real hash. Replace with a proper one.
            PasswordHash = "admin_password_hash",
            RoleId = adminRole.Id
        });
    }
}