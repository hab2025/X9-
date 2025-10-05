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
    [ObservableProperty]
    private ObservableCollection<Hall> _halls;

    [ObservableProperty]
    private Hall? _selectedHall;

    public HallManagementViewModel()
    {
        Halls = new ObservableCollection<Hall>();
        LoadHallsAsync();
    }

    private async Task LoadHallsAsync()
    {
        await using var db = new DesignTimeDbContextFactory().CreateDbContext(null);
        var hallsFromDb = await db.Halls.ToListAsync();
        Halls = new ObservableCollection<Hall>(hallsFromDb);
    }

    [RelayCommand]
    private void AddHall()
    {
        // TODO: Implement logic to open an "Add Hall" dialog.
    }

    [RelayCommand]
    private void EditHall()
    {
        // TODO: Implement logic to open an "Edit Hall" dialog for the SelectedHall.
    }

    [RelayCommand]
    private async Task DeleteHallAsync()
    {
        if (SelectedHall == null)
        {
            // In a real app, show a message to the user.
            return;
        }

        // In a real app, you would show a confirmation dialog here.

        await using var db = new DesignTimeDbContextFactory().CreateDbContext(null);
        db.Halls.Remove(SelectedHall);
        await db.SaveChangesAsync();

        // Refresh the list from the database to ensure consistency
        await LoadHallsAsync();
    }
}