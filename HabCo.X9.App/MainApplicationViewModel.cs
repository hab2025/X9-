using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HabCo.X9.App;

public partial class MainApplicationViewModel : ObservableObject
{
    private readonly IServiceProvider _services;
    private readonly IAuthenticationService _authService;

    [ObservableProperty]
    private object _currentView;

    public bool IsAdmin => _authService.CurrentUser?.Role?.Name == "Admin";
    public bool IsReception => _authService.CurrentUser?.Role?.Name == "Reception";
    public bool IsKitchen => _authService.CurrentUser?.Role?.Name == "Kitchen";

    public MainApplicationViewModel(IServiceProvider services, IAuthenticationService authService)
    {
        _services = services;
        _authService = authService;

        // Navigate to the default view for the current user's role
        NavigateToDefaultView();
    }

    private void NavigateToDefaultView()
    {
        // Define default views for different roles
        if (IsAdmin || IsReception)
        {
            NavigateToBookings();
        }
        else if (IsKitchen)
        {
            NavigateToKitchen();
        }
    }

    [RelayCommand]
    private void NavigateToBookings()
    {
        CurrentView = _services.GetRequiredService<BookingCalendarViewModel>();
    }

    [RelayCommand]
    private void NavigateToHalls()
    {
        CurrentView = _services.GetRequiredService<HallManagementViewModel>();
    }

    [RelayCommand]
    private void NavigateToInventory()
    {
        CurrentView = _services.GetRequiredService<InventoryViewModel>();
    }

    [RelayCommand]
    private void NavigateToKitchen()
    {
        CurrentView = _services.GetRequiredService<KitchenDashboardViewModel>();
    }

    [RelayCommand]
    private void NavigateToUserManagement()
    {
        CurrentView = _services.GetRequiredService<UserManagementViewModel>();
    }
}