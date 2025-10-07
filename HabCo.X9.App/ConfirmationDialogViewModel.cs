using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace HabCo.X9.App;

public partial class ConfirmationDialogViewModel : ObservableObject
{
    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    private string _message;

    public event Action<bool> CloseRequested = null!;

    public ConfirmationDialogViewModel(string title, string message)
    {
        Title = title;
        Message = message;
    }

    [RelayCommand]
    private void Yes()
    {
        CloseRequested?.Invoke(true);
    }

    [RelayCommand]
    private void No()
    {
        CloseRequested?.Invoke(false);
    }
}