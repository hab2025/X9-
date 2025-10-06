using CommunityToolkit.Mvvm.ComponentModel;
using HabCo.X9.Core;
using HabCo.X9.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace HabCo.X9.App;

using CommunityToolkit.Mvvm.Input;

public partial class BookingCalendarViewModel : ObservableObject
{
    private readonly AppDbContext _dbContext;
    private readonly IDialogService _dialogService;
    private List<Booking> _allBookings;

    [ObservableProperty]
    private DateTime _selectedDate = DateTime.Today;

    [ObservableProperty]
    private ObservableCollection<Booking> _selectedDayBookings;

    [ObservableProperty]
    private Booking? _selectedBooking;

    public BookingCalendarViewModel(AppDbContext dbContext, IDialogService dialogService)
    {
        _dbContext = dbContext;
        _dialogService = dialogService;
        _allBookings = new List<Booking>();
        SelectedDayBookings = new ObservableCollection<Booking>();
        LoadBookingsAsync();
    }

    private async Task LoadBookingsAsync()
    {
        // Include Hall data to avoid lazy loading issues
        _allBookings = await _dbContext.Bookings
            .Include(b => b.Hall)
            .ToListAsync();

        UpdateSelectedDayBookings();
    }

    private void UpdateSelectedDayBookings()
    {
        var bookings = _allBookings
            .Where(b => b.EventDay.Date == SelectedDate.Date)
            .ToList();

        SelectedDayBookings = new ObservableCollection<Booking>(bookings);
    }

    // This method is called by the MVVM toolkit whenever a property changes.
    partial void OnSelectedDateChanged(DateTime value)
    {
        UpdateSelectedDayBookings();
    }

    [RelayCommand]
    private async Task NewBookingAsync()
    {
        var newBooking = new Booking { EventDay = SelectedDate };
        var editorViewModel = new BookingEditorViewModel(_dbContext, newBooking);

        var savedBooking = await _dialogService.ShowDialogAsync<Booking>(editorViewModel);

        if (savedBooking != null)
        {
            _dbContext.Bookings.Add(savedBooking);
            await _dbContext.SaveChangesAsync();
            await LoadBookingsAsync(); // Refresh the list
        }
    }

    [RelayCommand]
    private async Task EditBookingAsync()
    {
        if (SelectedBooking == null) return;

        var editorViewModel = new BookingEditorViewModel(_dbContext, SelectedBooking);
        var savedBooking = await _dialogService.ShowDialogAsync<Booking>(editorViewModel);

        if (savedBooking != null)
        {
            _dbContext.Bookings.Update(savedBooking);
            await _dbContext.SaveChangesAsync();
            await LoadBookingsAsync(); // Refresh the list
        }
    }

    [RelayCommand]
    private async Task DeleteBookingAsync()
    {
        if (SelectedBooking == null) return;

        var confirmed = await _dialogService.ShowConfirmationDialogAsync(
            "Delete Booking",
            $"Are you sure you want to delete the booking for '{SelectedBooking.ClientName}' on {SelectedBooking.EventDay:d}?");

        if (confirmed)
        {
            _dbContext.Bookings.Remove(SelectedBooking);
            await _dbContext.SaveChangesAsync();
            await LoadBookingsAsync(); // Refresh the list
        }
    }
}