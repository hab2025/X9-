using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabCo.X9.Core;
using System;

namespace HabCo.X9.App;

public partial class HallEditorViewModel : ObservableObject
{
    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private int _capacity;

    [ObservableProperty]
    private decimal _rentalPrice;

    [ObservableProperty]
    private string? _errorMessage;

    public Hall Hall { get; }

    // This event will be used to close the dialog
    public event Action<bool> CloseRequested;

    public HallEditorViewModel(Hall hall)
    {
        Hall = hall;
        Name = hall.Name;
        Capacity = hall.Capacity;
        RentalPrice = hall.RentalPrice;
        Title = string.IsNullOrEmpty(hall.Name) ? "Add New Hall" : "Edit Hall";
    }

    [RelayCommand]
    private void Save()
    {
        // Basic validation
        if (string.IsNullOrWhiteSpace(Name) || Capacity <= 0 || RentalPrice < 0)
        {
            ErrorMessage = "All fields are required and must have valid values.";
            return;
        }

        // Update the underlying Hall object
        Hall.Name = Name;
        Hall.Capacity = Capacity;
        Hall.RentalPrice = RentalPrice;

        CloseRequested?.Invoke(true); // true indicates a save was performed
    }

    [RelayCommand]
    private void Cancel()
    {
        CloseRequested?.Invoke(false); // false indicates cancellation
    }
}