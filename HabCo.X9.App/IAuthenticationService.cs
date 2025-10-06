using HabCo.X9.Core;
using System.Threading.Tasks;

namespace HabCo.X9.App;

public interface IAuthenticationService
{
    User? CurrentUser { get; }
    bool IsLoggedIn { get; }
    Task<bool> LoginAsync(string username, string password);
    void Logout();
}