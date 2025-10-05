using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace HabCo.X9.App;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableObject _currentViewModel;

    private readonly LoginViewModel _loginViewModel;

    public MainWindowViewModel()
    {
        _loginViewModel = new LoginViewModel();
        _loginViewModel.PropertyChanged += OnLoginViewModelPropertyChanged;
        CurrentViewModel = _loginViewModel;
    }

    private void OnLoginViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(LoginViewModel.IsLoginSuccessful) && _loginViewModel.IsLoginSuccessful)
        {
            // Unsubscribe to prevent memory leaks
            _loginViewModel.PropertyChanged -= OnLoginViewModelPropertyChanged;

            // Switch to the main hall management view
            CurrentViewModel = new HallManagementViewModel();
        }
    }
}