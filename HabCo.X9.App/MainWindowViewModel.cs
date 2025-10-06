using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HabCo.X9.App;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly IServiceProvider _services;
    private readonly LoginViewModel _loginViewModel;

    [ObservableProperty]
    private object _currentViewModel;

    public MainWindowViewModel(IServiceProvider services)
    {
        _services = services;
        _loginViewModel = _services.GetRequiredService<LoginViewModel>();
        _loginViewModel.PropertyChanged += OnLoginViewModelPropertyChanged;
        CurrentViewModel = _loginViewModel;
    }

    private void OnLoginViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(LoginViewModel.IsLoginSuccessful) && _loginViewModel.IsLoginSuccessful)
        {
            _loginViewModel.PropertyChanged -= OnLoginViewModelPropertyChanged;
            CurrentViewModel = _services.GetRequiredService<MainApplicationViewModel>();
        }
    }
}