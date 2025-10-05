using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabCo.X9.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace HabCo.X9.App;

public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty]
    private string _username;

    [ObservableProperty]
    private string _password;

    [ObservableProperty]
    private string _errorMessage;

    // This property will be used by the main window to know when to switch views.
    [ObservableProperty]
    private bool _isLoginSuccessful = false;

    [RelayCommand]
    private async Task LoginAsync()
    {
        ErrorMessage = string.Empty;

        // This is a temporary way to get a DbContext instance.
        // It will be replaced with a proper dependency injection container later.
        await using var db = new DesignTimeDbContextFactory().CreateDbContext(null);

        var user = db.Users.FirstOrDefault(u => u.Username == Username);

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