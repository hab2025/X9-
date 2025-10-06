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
    public ObservableCollection<InventoryItem> AvailableInventoryItems { get; }
    public ObservableCollection<KitchenOrderItem> OrderItems { get; set; }

    [ObservableProperty]
    private InventoryItem? _selectedItemToAdd;

    [ObservableProperty]
    private int _quantityToAdd = 1;

    [ObservableProperty]
    private KitchenOrderItem? _selectedOrderItem;

    public Booking Booking { get; }
    public event Action<bool> CloseRequested;

    public BookingEditorViewModel(AppDbContext dbContext, Booking booking)
    {
        _dbContext = dbContext;
        Booking = booking;

        // Load data for ComboBoxes
        Halls = new ObservableCollection<Hall>(_dbContext.Halls.ToList());
        StatusOptions = new ObservableCollection<BookingStatus>((BookingStatus[])Enum.GetValues(typeof(BookingStatus)));
        AvailableInventoryItems = new ObservableCollection<InventoryItem>(_dbContext.InventoryItems.ToList());

        // Load existing order items or create a new list
        var existingOrder = _dbContext.KitchenOrders.Include(ko => ko.OrderItems).ThenInclude(oi => oi.InventoryItem).FirstOrDefault(ko => ko.BookingId == booking.Id);
        OrderItems = existingOrder != null
            ? new ObservableCollection<KitchenOrderItem>(existingOrder.OrderItems)
            : new ObservableCollection<KitchenOrderItem>();


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
    private void AddItem()
    {
        if (SelectedItemToAdd == null || QuantityToAdd <= 0) return;

        var existingItem = OrderItems.FirstOrDefault(oi => oi.InventoryItemId == SelectedItemToAdd.Id);
        if (existingItem != null)
        {
            existingItem.Quantity += QuantityToAdd;
        }
        else
        {
            OrderItems.Add(new KitchenOrderItem
            {
                InventoryItemId = SelectedItemToAdd.Id,
                InventoryItem = SelectedItemToAdd,
                Quantity = QuantityToAdd
            });
        }
    }

    [RelayCommand]
    private void RemoveItem()
    {
        if (SelectedOrderItem == null) return;
        OrderItems.Remove(SelectedOrderItem);
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

        // Find or create the KitchenOrder for this booking
        var kitchenOrder = _dbContext.KitchenOrders
                               .Include(ko => ko.OrderItems)
                               .FirstOrDefault(ko => ko.BookingId == Booking.Id);

        if (kitchenOrder == null && OrderItems.Any())
        {
            kitchenOrder = new KitchenOrder { BookingId = Booking.Id, IssueDate = DateTime.UtcNow };
            _dbContext.KitchenOrders.Add(kitchenOrder);
        }

        if (kitchenOrder != null)
        {
            kitchenOrder.Status = KitchenOrderStatus.Pending; // Or logic to determine status

            // Clear old items and add new ones
            kitchenOrder.OrderItems.Clear();
            foreach (var item in OrderItems)
            {
                kitchenOrder.OrderItems.Add(new KitchenOrderItem
                {
                    InventoryItemId = item.InventoryItemId,
                    Quantity = item.Quantity
                });
            }
        }

        CloseRequested?.Invoke(true);
    }

    [RelayCommand]
    private void Cancel()
    {
        CloseRequested?.Invoke(false);
    }
}