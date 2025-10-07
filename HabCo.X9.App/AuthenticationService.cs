using BCrypt.Net;
using HabCo.X9.Core;
using HabCo.X9.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace HabCo.X9.App;

public class AuthenticationService : IAuthenticationService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<AuthenticationService> _logger;

    public User? CurrentUser { get; private set; }
    public bool IsLoggedIn => CurrentUser != null;

    public AuthenticationService(AppDbContext dbContext, ILogger<AuthenticationService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        var user = await _dbContext.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user != null && user.IsActive && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            CurrentUser = user;
            _logger.LogInformation("User '{Username}' logged in successfully.", username);
            return true;
        }

        _logger.LogWarning("Failed login attempt for username: '{Username}'.", username);
        return false;
    }

    public void Logout()
    {
        _logger.LogInformation("User '{Username}' logged out.", CurrentUser?.Username);
        CurrentUser = null;
    }
}