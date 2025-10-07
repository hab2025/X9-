using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabCo.X9.Core;
using HabCo.X9.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace HabCo.X9.App;

public partial class UserEditorViewModel : ObservableValidator
{
    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Username is required.")]
    private string _username = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private bool _isActive;

    [ObservableProperty]
    [Required(ErrorMessage = "Role is required.")]
    private Role? _selectedRole;

    [ObservableProperty]
    private string? _errorMessage;

    public ObservableCollection<Role> Roles { get; }
    public User User { get; }
    public event Action<bool> CloseRequested = null!;

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
        ErrorMessage = null;
        ValidateAllProperties();

        var validationErrors = GetErrors().Select(e => e.ErrorMessage).ToList();

        if (User.Id == 0 && string.IsNullOrWhiteSpace(Password))
        {
            validationErrors.Add("Password is required for new users.");
        }

        if (validationErrors.Any())
        {
            ErrorMessage = string.Join(Environment.NewLine, validationErrors);
            return;
        }

        User.Username = Username;
        User.IsActive = IsActive;
        User.RoleId = SelectedRole!.Id;

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