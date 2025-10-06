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
    private DiscountType _discountType;

    [ObservableProperty]
    private decimal _discountValue;

    [ObservableProperty]
    private BookingStatus _status;

    [ObservableProperty]
    private string? _errorMessage;

    public ObservableCollection<Hall> Halls { get; }
    public ObservableCollection<BookingStatus> StatusOptions { get; }
    public ObservableCollection<DiscountType> DiscountTypes { get; }
    public ObservableCollection<InventoryItem> AvailableInventoryItems { get; }
    public ObservableCollection<KitchenOrderItem> OrderItems { get; set; }
    public ObservableCollection<SelectableServiceViewModel> Services { get; }

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
        DiscountTypes = new ObservableCollection<DiscountType>((DiscountType[])Enum.GetValues(typeof(DiscountType)));
        AvailableInventoryItems = new ObservableCollection<InventoryItem>(_dbContext.InventoryItems.ToList());

        var existingOrder = _dbContext.KitchenOrders.Include(ko => ko.OrderItems).ThenInclude(oi => oi.InventoryItem).FirstOrDefault(ko => ko.BookingId == booking.Id);
        OrderItems = existingOrder != null
            ? new ObservableCollection<KitchenOrderItem>(existingOrder.OrderItems)
            : new ObservableCollection<KitchenOrderItem>();

        var allServices = _dbContext.Services.ToList();
        var bookingServiceIds = _dbContext.BookingServices
            .Where(bs => bs.BookingId == booking.Id)
            .Select(bs => bs.ServiceId)
            .ToHashSet();

        Services = new ObservableCollection<SelectableServiceViewModel>(
            allServices.Select(s => new SelectableServiceViewModel(s)
            {
                IsSelected = bookingServiceIds.Contains(s.Id)
            }));

        ClientName = booking.ClientName;
        ClientPhone = booking.ClientPhone;
        ClientEmail = booking.ClientEmail;
        EventDay = booking.EventDay == default ? DateTimeOffset.Now : new DateTimeOffset(booking.EventDay);
        StartTime = booking.StartTime;
        EndTime = booking.EndTime;
        SelectedHall = Halls.FirstOrDefault(h => h.Id == booking.HallId) ?? Halls.FirstOrDefault();
        Status = booking.Status;
        DiscountType = booking.DiscountType;
        DiscountValue = booking.DiscountValue;
        Title = booking.Id == 0 ? "Add New Booking" : "Edit Booking";

        foreach (var serviceVM in Services)
        {
            serviceVM.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(SelectableServiceViewModel.IsSelected))
                {
                    UpdateTotalCost();
                }
            };
        }

        UpdateTotalCost();
    }

    private void UpdateTotalCost()
    {
        var hallCost = SelectedHall?.RentalPrice ?? 0;
        var servicesCost = Services.Where(s => s.IsSelected).Sum(s => s.Service.Price);
        var grossTotal = hallCost + servicesCost;

        var discountAmount = 0m;
        if (DiscountType == DiscountType.Fixed)
        {
            discountAmount = DiscountValue;
        }
        else if (DiscountType == DiscountType.Percentage)
        {
            discountAmount = grossTotal * (DiscountValue / 100);
        }

        TotalCost = grossTotal - discountAmount;
    }

    partial void OnSelectedHallChanged(Hall? value) => UpdateTotalCost();
    partial void OnDiscountTypeChanged(DiscountType value) => UpdateTotalCost();
    partial void OnDiscountValueChanged(decimal value) => UpdateTotalCost();

    [RelayCommand]
    private void AddItem()
    {
        if (SelectedItemToAdd == null || QuantityToAdd <= 0) return;
        var existingItem = OrderItems.FirstOrDefault(oi => oi.InventoryItemId == SelectedItemToAdd.Id);
        if (existingItem != null) { existingItem.Quantity += QuantityToAdd; }
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
        if (string.IsNullOrWhiteSpace(ClientName) || SelectedHall == null || !EventDay.HasValue)
        {
            ErrorMessage = "Client Name, Hall, and Event Date are required.";
            return;
        }

        Booking.ClientName = ClientName;
        Booking.ClientPhone = ClientPhone;
        Booking.ClientEmail = ClientEmail;
        Booking.EventDay = EventDay.Value.DateTime;
        Booking.StartTime = StartTime ?? TimeSpan.Zero;
        Booking.EndTime = EndTime ?? TimeSpan.Zero;
        Booking.HallId = SelectedHall.Id;
        Booking.TotalCost = TotalCost;
        Booking.DiscountType = DiscountType;
        Booking.DiscountValue = DiscountValue;
        Booking.Status = Status;

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
            kitchenOrder.Status = KitchenOrderStatus.Pending;
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

        var existingServices = _dbContext.BookingServices.Where(bs => bs.BookingId == Booking.Id);
        _dbContext.BookingServices.RemoveRange(existingServices);
        var selectedServices = Services.Where(s => s.IsSelected).Select(s => s.Service);
        foreach (var service in selectedServices)
        {
            _dbContext.BookingServices.Add(new BookingService { BookingId = Booking.Id, ServiceId = service.Id });
        }

        CloseRequested?.Invoke(true);
    }

    [RelayCommand]
    private void Cancel()
    {
        CloseRequested?.Invoke(false);
    }
}