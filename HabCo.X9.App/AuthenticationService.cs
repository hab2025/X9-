using BCrypt.Net;
using HabCo.X9.Core;
using HabCo.X9.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HabCo.X9.App;

public class AuthenticationService : IAuthenticationService
{
    private readonly AppDbContext _dbContext;

    public User? CurrentUser { get; private set; }
    public bool IsLoggedIn => CurrentUser != null;

    public AuthenticationService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        var user = await _dbContext.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user != null && user.IsActive && BCrypt.Verify(password, user.PasswordHash))
        {
            CurrentUser = user;
            return true;
        }

        return false;
    }

    public void Logout()
    {
        CurrentUser = null;
    }
}