using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabCo.X9.Core;
using HabCo.X9.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HabCo.X9.App;

public partial class HallManagementViewModel : ObservableObject
{
    private readonly AppDbContext _dbContext;
    private readonly IDialogService _dialogService;

    [ObservableProperty]
    private ObservableCollection<Hall> _halls;

    [ObservableProperty]
    private Hall? _selectedHall;

    public HallManagementViewModel(AppDbContext dbContext, IDialogService dialogService)
    {
        _dbContext = dbContext;
        _dialogService = dialogService;
        Halls = new ObservableCollection<Hall>();
        _ = LoadHallsAsync();
    }

    private async Task LoadHallsAsync()
    {
        var hallsFromDb = await _dbContext.Halls.ToListAsync();
        Halls = new ObservableCollection<Hall>(hallsFromDb);
    }

    [RelayCommand]
    private async Task AddHallAsync()
    {
        var editorViewModel = new HallEditorViewModel(new Hall());
        var savedHall = await _dialogService.ShowDialogAsync<Hall>(editorViewModel);

        if (savedHall != null)
        {
            _dbContext.Halls.Add(savedHall);
            await _dbContext.SaveChangesAsync();
            await LoadHallsAsync();
        }
    }

    [RelayCommand]
    private async Task EditHallAsync()
    {
        if (SelectedHall == null) return;

        var editorViewModel = new HallEditorViewModel(SelectedHall);
        var savedHall = await _dialogService.ShowDialogAsync<Hall>(editorViewModel);

        if (savedHall != null)
        {
            _dbContext.Halls.Update(savedHall);
            await _dbContext.SaveChangesAsync();
            await LoadHallsAsync();
        }
    }

    [RelayCommand]
    private async Task DeleteHallAsync()
    {
        if (SelectedHall == null) return;

        var confirmed = await _dialogService.ShowConfirmationDialogAsync(
            "Delete Hall",
            $"Are you sure you want to delete '{SelectedHall.Name}'?");

        if (confirmed)
        {
            _dbContext.Halls.Remove(SelectedHall);
            await _dbContext.SaveChangesAsync();
            await LoadHallsAsync();
        }
    }
}