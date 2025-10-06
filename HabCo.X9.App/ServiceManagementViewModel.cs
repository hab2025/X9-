using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabCo.X9.Core;
using HabCo.X9.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HabCo.X9.App;

public partial class ServiceManagementViewModel : ObservableObject
{
    private readonly AppDbContext _dbContext;
    private readonly IDialogService _dialogService;

    [ObservableProperty]
    private ObservableCollection<Service> _services;

    [ObservableProperty]
    private Service? _selectedService;

    public ServiceManagementViewModel(AppDbContext dbContext, IDialogService dialogService)
    {
        _dbContext = dbContext;
        _dialogService = dialogService;
        Services = new ObservableCollection<Service>();
        LoadServicesAsync();
    }

    private async Task LoadServicesAsync()
    {
        var servicesFromDb = await _dbContext.Services.ToListAsync();
        Services = new ObservableCollection<Service>(servicesFromDb);
    }

    [RelayCommand]
    private async Task AddServiceAsync()
    {
        var editorViewModel = new ServiceEditorViewModel(new Service());
        var savedService = await _dialogService.ShowDialogAsync<Service>(editorViewModel);

        if (savedService != null)
        {
            _dbContext.Services.Add(savedService);
            await _dbContext.SaveChangesAsync();
            await LoadServicesAsync(); // Refresh the list
        }
    }

    [RelayCommand]
    private async Task EditServiceAsync()
    {
        if (SelectedService == null) return;

        var editorViewModel = new ServiceEditorViewModel(SelectedService);
        var savedService = await _dialogService.ShowDialogAsync<Service>(editorViewModel);

        if (savedService != null)
        {
            _dbContext.Services.Update(savedService);
            await _dbContext.SaveChangesAsync();
            await LoadServicesAsync(); // Refresh the list
        }
    }

    [RelayCommand]
    private async Task DeleteServiceAsync()
    {
        if (SelectedService == null) return;

        var confirmed = await _dialogService.ShowConfirmationDialogAsync(
            "Delete Service",
            $"Are you sure you want to delete the service '{SelectedService.Name}'?");

        if (confirmed)
        {
            _dbContext.Services.Remove(SelectedService);
            await _dbContext.SaveChangesAsync();
            await LoadServicesAsync(); // Refresh the list
        }
    }
}