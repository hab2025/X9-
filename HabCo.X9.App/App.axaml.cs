using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using HabCo.X9.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace HabCo.X9.App;

public partial class App : Application
{
    public new static App? Current => Application.Current as App;
    public IServiceProvider? Services { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Services = ConfigureServices();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = Services.GetRequiredService<MainWindowViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        // Add Logging
        services.AddLogging(builder =>
        {
            builder.AddSerilog(dispose: true);
        });

        // Register DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=hab_x9.db"));

        // Register Services
        services.AddTransient<IDialogService, DialogService>();
        services.AddSingleton<IAuthenticationService, AuthenticationService>();
        services.AddTransient<IReportService, ReportService>();

        // Register ViewModels
        services.AddTransient<LoginViewModel>();
        services.AddTransient<HallManagementViewModel>();
        services.AddTransient<BookingCalendarViewModel>();
        services.AddTransient<InventoryViewModel>();
        services.AddTransient<KitchenDashboardViewModel>();
        services.AddTransient<UserManagementViewModel>();
        services.AddTransient<ServiceManagementViewModel>();
        services.AddTransient<ReportsViewModel>();
        services.AddTransient<HallEditorViewModel>();
        services.AddTransient<BookingEditorViewModel>();
        services.AddTransient<InventoryItemEditorViewModel>();
        services.AddTransient<UserEditorViewModel>();
        services.AddTransient<ServiceEditorViewModel>();
        services.AddTransient<ConfirmationDialogViewModel>();

        services.AddSingleton<MainApplicationViewModel>();
        services.AddSingleton<MainWindowViewModel>();

        return services.BuildServiceProvider();
    }
}