using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HabCo.X9.App;

public partial class LoginViewModel : ObservableValidator
{
    private readonly IAuthenticationService _authService;

    [ObservableProperty]
    [Required(ErrorMessage = "Username is required.")]
    private string _username = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Password is required.")]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private bool _isLoginSuccessful = false;

    public LoginViewModel(IAuthenticationService authService)
    {
        _authService = authService;
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        ValidateAllProperties();
        if (HasErrors)
        {
            ErrorMessage = string.Join(Environment.NewLine, GetErrors().Select(e => e.ErrorMessage));
            return;
        }

        ErrorMessage = string.Empty;
        var success = await _authService.LoginAsync(Username, Password);

        if (success)
        {
            IsLoginSuccessful = true;
        }
        else
        {
            ErrorMessage = "Invalid username or password, or account is inactive.";
        }
    }
}