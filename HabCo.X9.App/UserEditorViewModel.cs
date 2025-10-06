using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabCo.X9.Core;
using HabCo.X9.Infrastructure;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace HabCo.X9.App;

public partial class UserEditorViewModel : ObservableObject
{
    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    private string _username;

    [ObservableProperty]
    private string _password;

    [ObservableProperty]
    private bool _isActive;

    [ObservableProperty]
    private Role? _selectedRole;

    [ObservableProperty]
    private string? _errorMessage;

    public ObservableCollection<Role> Roles { get; }
    public User User { get; }
    public event Action<bool> CloseRequested;

    public UserEditorViewModel(AppDbContext dbContext, User user)
    {
        User = user;

        Roles = new ObservableCollection<Role>(dbContext.Roles.ToList());

        Username = user.Username;
        IsActive = user.IsActive;
        SelectedRole = Roles.FirstOrDefault(r => r.Id == user.RoleId);

        Title = user.Id == 0 ? "Add New User" : "Edit User";
    }

    [RelayCommand]
    private void Save()
    {
        if (string.IsNullOrWhiteSpace(Username) || SelectedRole == null)
        {
            ErrorMessage = "Username and Role are required.";
            return;
        }

        // For a new user, a password is required.
        if (User.Id == 0 && string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Password is required for new users.";
            return;
        }

        User.Username = Username;
        User.IsActive = IsActive;
        User.RoleId = SelectedRole.Id;

        // Only hash and update the password if a new one was entered.
        if (!string.IsNullOrWhiteSpace(Password))
        {
            User.PasswordHash = BCrypt.Net.BCrypt.HashPassword(Password);
        }

        CloseRequested?.Invoke(true);
    }

    [RelayCommand]
    private void Cancel()
    {
        CloseRequested?.Invoke(false);
    }
}