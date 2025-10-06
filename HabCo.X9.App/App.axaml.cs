using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using HabCo.X9.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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

        // Register DbContext with a scoped lifetime
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=hab_x9.db"));

        // Register Services
        services.AddTransient<IDialogService, DialogService>();

        // Register ViewModels
        // Transient: a new instance is created every time one is requested.
        services.AddTransient<LoginViewModel>();
        services.AddTransient<HallManagementViewModel>();

        // Singleton: a single instance is created and used for the lifetime of the application.
        services.AddSingleton<MainWindowViewModel>();

        return services.BuildServiceProvider();
    }
}