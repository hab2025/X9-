using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabCo.X9.Core;
using HabCo.X9.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HabCo.X9.App;

public partial class BookingEditorViewModel : ObservableObject
{
    private readonly AppDbContext _dbContext;

    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    private string _clientName;

    [ObservableProperty]
    private string _clientPhone;

    [ObservableProperty]
    private string _clientEmail;

    [ObservableProperty]
    private DateTimeOffset? _eventDay;

    [ObservableProperty]
    private TimeSpan? _startTime;

    [ObservableProperty]
    private TimeSpan? _endTime;

    [ObservableProperty]
    private Hall _selectedHall;

    [ObservableProperty]
    private decimal _totalCost;

    [ObservableProperty]
    private BookingStatus _status;

    [ObservableProperty]
    private string? _errorMessage;

    public ObservableCollection<Hall> Halls { get; }
    public ObservableCollection<BookingStatus> StatusOptions { get; }

    public Booking Booking { get; }
    public event Action<bool> CloseRequested;

    public BookingEditorViewModel(AppDbContext dbContext, Booking booking)
    {
        _dbContext = dbContext;
        Booking = booking;

        // Load data for ComboBoxes
        Halls = new ObservableCollection<Hall>(_dbContext.Halls.ToList());
        StatusOptions = new ObservableCollection<BookingStatus>((BookingStatus[])Enum.GetValues(typeof(BookingStatus)));

        // Initialize properties from the booking object
        ClientName = booking.ClientName;
        ClientPhone = booking.ClientPhone;
        ClientEmail = booking.ClientEmail;
        EventDay = booking.EventDay == default ? DateTimeOffset.Now : new DateTimeOffset(booking.EventDay);
        StartTime = booking.StartTime;
        EndTime = booking.EndTime;
        SelectedHall = Halls.FirstOrDefault(h => h.Id == booking.HallId) ?? Halls.FirstOrDefault();
        TotalCost = booking.TotalCost;
        Status = booking.Status;

        Title = booking.Id == 0 ? "Add New Booking" : "Edit Booking";
    }

    [RelayCommand]
    private void Save()
    {
        // TODO: Add robust validation
        if (string.IsNullOrWhiteSpace(ClientName) || SelectedHall == null || !EventDay.HasValue)
        {
            ErrorMessage = "Client Name, Hall, and Event Date are required.";
            return;
        }

        // Update the underlying Booking object
        Booking.ClientName = ClientName;
        Booking.ClientPhone = ClientPhone;
        Booking.ClientEmail = ClientEmail;
        Booking.EventDay = EventDay.Value.DateTime;
        Booking.StartTime = StartTime ?? TimeSpan.Zero;
        Booking.EndTime = EndTime ?? TimeSpan.Zero;
        Booking.HallId = SelectedHall.Id;
        Booking.TotalCost = TotalCost;
        Booking.Status = Status;

        CloseRequested?.Invoke(true);
    }

    [RelayCommand]
    private void Cancel()
    {
        CloseRequested?.Invoke(false);
    }
}