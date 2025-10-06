using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabCo.X9.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;

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

        if (user != null && BCrypt.Verify(Password, user.PasswordHash))
        {
            IsLoginSuccessful = true;
        }
        else
        {
            ErrorMessage = "Invalid username or password.";
        }
    }
}