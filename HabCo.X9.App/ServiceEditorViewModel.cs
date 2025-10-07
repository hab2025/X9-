using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabCo.X9.Core;
using System;

namespace HabCo.X9.App;

public partial class ServiceEditorViewModel : ObservableObject
{
    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private decimal _price;

    [ObservableProperty]
    private string? _errorMessage;

    public Service Service { get; }
    public event Action<bool> CloseRequested = null!;

    public ServiceEditorViewModel(Service service)
    {
        Service = service;
        Name = service.Name;
        Price = service.Price;
        Title = service.Id == 0 ? "Add New Service" : "Edit Service";
    }

    [RelayCommand]
    private void Save()
    {
        if (string.IsNullOrWhiteSpace(Name) || Price < 0)
        {
            ErrorMessage = "Service Name and a valid Price are required.";
            return;
        }

        Service.Name = Name;
        Service.Price = Price;

        CloseRequested?.Invoke(true);
    }

    [RelayCommand]
    private void Cancel()
    {
        CloseRequested?.Invoke(false);
    }
}