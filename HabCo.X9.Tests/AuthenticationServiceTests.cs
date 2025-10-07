using HabCo.X9.App;
using HabCo.X9.Core;
using HabCo.X9.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace HabCo.X9.Tests;

public class AuthenticationServiceTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
            .Options;
        var dbContext = new AppDbContext(options);
        return dbContext;
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ShouldReturnTrueAndSetCurrentUser()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var loggerMock = new Mock<ILogger<AuthenticationService>>();
        var role = new Role { Id = 1, Name = "Admin" };
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password123");
        var user = new User { Id = 1, Username = "admin", PasswordHash = hashedPassword, Role = role, IsActive = true };
        dbContext.Roles.Add(role);
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var authService = new AuthenticationService(dbContext, loggerMock.Object);

        // Act
        var result = await authService.LoginAsync("admin", "password123");

        // Assert
        Assert.True(result);
        Assert.NotNull(authService.CurrentUser);
        Assert.Equal("admin", authService.CurrentUser.Username);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidPassword_ShouldReturnFalse()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var loggerMock = new Mock<ILogger<AuthenticationService>>();
        var role = new Role { Id = 1, Name = "Admin" };
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password123");
        var user = new User { Id = 1, Username = "admin", PasswordHash = hashedPassword, Role = role, IsActive = true };
        dbContext.Roles.Add(role);
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var authService = new AuthenticationService(dbContext, loggerMock.Object);

        // Act
        var result = await authService.LoginAsync("admin", "wrongpassword");

        // Assert
        Assert.False(result);
        Assert.Null(authService.CurrentUser);
    }

    [Fact]
    public async Task LoginAsync_WithInactiveUser_ShouldReturnFalse()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var loggerMock = new Mock<ILogger<AuthenticationService>>();
        var role = new Role { Id = 1, Name = "Admin" };
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password123");
        var user = new User { Id = 1, Username = "admin", PasswordHash = hashedPassword, Role = role, IsActive = false };
        dbContext.Roles.Add(role);
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var authService = new AuthenticationService(dbContext, loggerMock.Object);

        // Act
        var result = await authService.LoginAsync("admin", "password123");

        // Assert
        Assert.False(result);
        Assert.Null(authService.CurrentUser);
    }
}