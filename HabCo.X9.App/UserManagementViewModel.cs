using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabCo.X9.Core;
using HabCo.X9.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HabCo.X9.App;

public partial class UserManagementViewModel : ObservableObject
{
    private readonly AppDbContext _dbContext;
    private readonly IDialogService _dialogService;

    [ObservableProperty]
    private ObservableCollection<User> _users;

    [ObservableProperty]
    private User? _selectedUser;

    public UserManagementViewModel(AppDbContext dbContext, IDialogService dialogService)
    {
        _dbContext = dbContext;
        _dialogService = dialogService;
        Users = new ObservableCollection<User>();
        LoadUsersAsync();
    }

    private async Task LoadUsersAsync()
    {
        var usersFromDb = await _dbContext.Users
            .Include(u => u.Role)
            .ToListAsync();

        Users = new ObservableCollection<User>(usersFromDb);
    }

    [RelayCommand]
    private async Task AddUserAsync()
    {
        var editorViewModel = new UserEditorViewModel(_dbContext, new User());
        var savedUser = await _dialogService.ShowDialogAsync<User>(editorViewModel);

        if (savedUser != null)
        {
            _dbContext.Users.Add(savedUser);
            await _dbContext.SaveChangesAsync();
            await LoadUsersAsync();
        }
    }

    [RelayCommand]
    private async Task EditUserAsync()
    {
        if (SelectedUser == null) return;

        var editorViewModel = new UserEditorViewModel(_dbContext, SelectedUser);
        var savedUser = await _dialogService.ShowDialogAsync<User>(editorViewModel);

        if (savedUser != null)
        {
            _dbContext.Users.Update(savedUser);
            await _dbContext.SaveChangesAsync();
            await LoadUsersAsync();
        }
    }

    [RelayCommand]
    private async Task ToggleActiveStatusAsync()
    {
        if (SelectedUser == null) return;

        SelectedUser.IsActive = !SelectedUser.IsActive;
        _dbContext.Users.Update(SelectedUser);
        await _dbContext.SaveChangesAsync();
        await LoadUsersAsync();
    }
}