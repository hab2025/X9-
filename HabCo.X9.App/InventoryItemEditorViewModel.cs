using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabCo.X9.Core;
using HabCo.X9.Infrastructure;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace HabCo.X9.App;

public partial class InventoryItemEditorViewModel : ObservableObject
{
    private readonly AppDbContext _dbContext;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private int _quantity;

    [ObservableProperty]
    private string _unit = string.Empty;

    [ObservableProperty]
    private int _reorderLevel;

    [ObservableProperty]
    private Supplier? _selectedSupplier;

    [ObservableProperty]
    private string? _errorMessage;

    public ObservableCollection<Supplier> Suppliers { get; }
    public InventoryItem Item { get; }
    public event Action<bool> CloseRequested = null!;

    public InventoryItemEditorViewModel(AppDbContext dbContext, InventoryItem item)
    {
        _dbContext = dbContext;
        Item = item;

        Suppliers = new ObservableCollection<Supplier>(_dbContext.Suppliers.ToList());

        Name = item.Name;
        Description = item.Description;
        Quantity = item.Quantity;
        Unit = item.Unit;
        ReorderLevel = item.ReorderLevel;
        SelectedSupplier = Suppliers.FirstOrDefault(s => s.Id == item.SupplierId);

        Title = item.Id == 0 ? "Add New Inventory Item" : "Edit Inventory Item";
    }

    [RelayCommand]
    private void Save()
    {
        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Unit))
        {
            ErrorMessage = "Item Name and Unit are required.";
            return;
        }

        Item.Name = Name;
        Item.Description = Description;
        Item.Quantity = Quantity;
        Item.Unit = Unit;
        Item.ReorderLevel = ReorderLevel;
        Item.SupplierId = SelectedSupplier?.Id;

        CloseRequested?.Invoke(true);
    }

    [RelayCommand]
    private void Cancel()
    {
        CloseRequested?.Invoke(false);
    }
}