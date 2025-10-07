using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabCo.X9.Core;
using System;

namespace HabCo.X9.App;

public partial class HallEditorViewModel : ObservableObject
{
    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private int _capacity;

    [ObservableProperty]
    private decimal _rentalPrice;

    [ObservableProperty]
    private string? _errorMessage;

    public Hall Hall { get; }
    public event Action<bool> CloseRequested = null!;

    public HallEditorViewModel(Hall hall)
    {
        Hall = hall;
        Name = hall.Name;
        Capacity = hall.Capacity;
        RentalPrice = hall.RentalPrice;
        Title = hall.Id == 0 ? "Add New Hall" : "Edit Hall";
    }

    [RelayCommand]
    private void Save()
    {
        if (string.IsNullOrWhiteSpace(Name) || Capacity <= 0)
        {
            ErrorMessage = "Hall Name and a valid Capacity are required.";
            return;
        }

        Hall.Name = Name;
        Hall.Capacity = Capacity;
        Hall.RentalPrice = RentalPrice;

        CloseRequested?.Invoke(true);
    }

    [RelayCommand]
    private void Cancel()
    {
        CloseRequested?.Invoke(false);
    }
}