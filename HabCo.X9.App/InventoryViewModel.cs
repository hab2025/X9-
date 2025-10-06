using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabCo.X9.Core;
using HabCo.X9.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HabCo.X9.App;

public partial class InventoryViewModel : ObservableObject
{
    private readonly AppDbContext _dbContext;
    private readonly IDialogService _dialogService;

    [ObservableProperty]
    private ObservableCollection<InventoryItem> _items;

    [ObservableProperty]
    private InventoryItem? _selectedItem;

    public InventoryViewModel(AppDbContext dbContext, IDialogService dialogService)
    {
        _dbContext = dbContext;
        _dialogService = dialogService;
        Items = new ObservableCollection<InventoryItem>();
        LoadItemsAsync();
    }

    private async Task LoadItemsAsync()
    {
        // Include Supplier data to display the supplier's name
        var itemsFromDb = await _dbContext.InventoryItems
            .Include(i => i.Supplier)
            .ToListAsync();

        Items = new ObservableCollection<InventoryItem>(itemsFromDb);
    }

    [RelayCommand]
    private async Task AddItemAsync()
    {
        var editorViewModel = new InventoryItemEditorViewModel(_dbContext, new InventoryItem());
        var savedItem = await _dialogService.ShowDialogAsync<InventoryItem>(editorViewModel);

        if (savedItem != null)
        {
            _dbContext.InventoryItems.Add(savedItem);
            await _dbContext.SaveChangesAsync();
            await LoadItemsAsync(); // Refresh the list
        }
    }

    [RelayCommand]
    private async Task EditItemAsync()
    {
        if (SelectedItem == null) return;

        var editorViewModel = new InventoryItemEditorViewModel(_dbContext, SelectedItem);
        var savedItem = await _dialogService.ShowDialogAsync<InventoryItem>(editorViewModel);

        if (savedItem != null)
        {
            _dbContext.InventoryItems.Update(savedItem);
            await _dbContext.SaveChangesAsync();
            await LoadItemsAsync(); // Refresh the list
        }
    }

    [RelayCommand]
    private async Task DeleteItemAsync()
    {
        if (SelectedItem == null) return;

        var confirmed = await _dialogService.ShowConfirmationDialogAsync(
            "Delete Item",
            $"Are you sure you want to delete '{SelectedItem.Name}'?");

        if (confirmed)
        {
            _dbContext.InventoryItems.Remove(SelectedItem);
            await _dbContext.SaveChangesAsync();
            await LoadItemsAsync(); // Refresh the list
        }
    }
}