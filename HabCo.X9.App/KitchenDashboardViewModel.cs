using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabCo.X9.Core;
using HabCo.X9.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace HabCo.X9.App;

public partial class KitchenDashboardViewModel : ObservableObject
{
    private readonly AppDbContext _dbContext;

    [ObservableProperty]
    private ObservableCollection<KitchenOrder> _orders;

    [ObservableProperty]
    private KitchenOrder? _selectedOrder;

    public KitchenDashboardViewModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        Orders = new ObservableCollection<KitchenOrder>();
        _ = LoadOrdersAsync();
    }

    private async Task LoadOrdersAsync()
    {
        // Load all non-completed orders, ordered by the event date
        var ordersFromDb = await _dbContext.KitchenOrders
            .Include(ko => ko.Booking)
            .Include(ko => ko.OrderItems)
                .ThenInclude(oi => oi.InventoryItem)
            .Where(ko => ko.Status != KitchenOrderStatus.Completed)
            .OrderBy(ko => ko.Booking.EventDay)
            .ToListAsync();

        Orders = new ObservableCollection<KitchenOrder>(ordersFromDb);
    }

    [RelayCommand]
    private async Task SetStatus(KitchenOrderStatus status)
    {
        if (SelectedOrder == null) return;

        SelectedOrder.Status = status;
        _dbContext.KitchenOrders.Update(SelectedOrder);
        await _dbContext.SaveChangesAsync();

        // Refresh the list to remove completed orders if necessary
        await LoadOrdersAsync();
    }
}