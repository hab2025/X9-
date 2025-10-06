using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace HabCo.X9.App;

using Microsoft.Extensions.DependencyInjection;
using System;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableObject _currentViewModel;

    private readonly IServiceProvider _services;
    private readonly LoginViewModel _loginViewModel;

    public MainWindowViewModel(IServiceProvider services)
    {
        _services = services;
        _loginViewModel = _services.GetRequiredService<LoginViewModel>();
        _loginViewModel.PropertyChanged += OnLoginViewModelPropertyChanged;
        CurrentViewModel = _loginViewModel;
    }

    private void OnLoginViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(LoginViewModel.IsLoginSuccessful) && _loginViewModel.IsLoginSuccessful)
        {
            // Unsubscribe to prevent memory leaks
            _loginViewModel.PropertyChanged -= OnLoginViewModelPropertyChanged;

            // Switch to the main hall management view by resolving it from the DI container
            CurrentViewModel = _services.GetRequiredService<HallManagementViewModel>();
        }
    }
}