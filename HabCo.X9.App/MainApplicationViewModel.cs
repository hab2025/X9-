using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HabCo.X9.App;

public partial class MainApplicationViewModel : ObservableObject
{
    private readonly IServiceProvider _services;

    [ObservableProperty]
    private object _currentView;

    public MainApplicationViewModel(IServiceProvider services)
    {
        _services = services;
        // Start with the booking calendar view by default
        CurrentView = _services.GetRequiredService<BookingCalendarViewModel>();
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
}