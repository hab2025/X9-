using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabCo.X9.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HabCo.X9.App;

public partial class LoginViewModel : ObservableObject
{
    private readonly AppDbContext _dbContext;

    [ObservableProperty]
    private string _username;

    [ObservableProperty]
    private string _password;

    [ObservableProperty]
    private string _errorMessage;

    [ObservableProperty]
    private bool _isLoginSuccessful = false;

    public LoginViewModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        ErrorMessage = string.Empty;

        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == Username);

        // NOTE: This is a placeholder password check and is NOT secure.
        // It will be replaced with a proper hashing mechanism.
        if (user != null && user.PasswordHash == Password)
        {
            IsLoginSuccessful = true;
        }
        else
        {
            ErrorMessage = "Invalid username or password.";
        }
    }
}