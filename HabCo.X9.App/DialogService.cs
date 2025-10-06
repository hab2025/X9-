using Avalonia.Controls;
using System;
using System.Threading.Tasks;

namespace HabCo.X9.App;

public class DialogService : IDialogService
{
    // A simple way to get the main window. In a real app, this might be handled by an application service.
    private static Window? MainWindow => App.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;

    public Task<TResult?> ShowDialogAsync<TResult>(object viewModel) where TResult : class
    {
        var dialog = new DialogWindow
        {
            DataContext = viewModel
        };

        var tcs = new TaskCompletionSource<TResult?>();

        if (viewModel is HallEditorViewModel hallEditor)
        {
            hallEditor.CloseRequested += (saved) =>
            {
                tcs.SetResult(saved ? hallEditor.Hall as TResult : null);
                dialog.Close();
            };
        }
        else if (viewModel is InventoryItemEditorViewModel inventoryEditor)
        {
            inventoryEditor.CloseRequested += (saved) =>
            {
                tcs.SetResult(saved ? inventoryEditor.Item as TResult : null);
                dialog.Close();
            };
        }
        else if (viewModel is BookingEditorViewModel bookingEditor)
        {
            bookingEditor.CloseRequested += (saved) =>
            {
                tcs.SetResult(saved ? bookingEditor.Booking as TResult : null);
                dialog.Close();
            };
        }
        else
        {
            // Handle other view model types if necessary
            dialog.Closed += (s, e) => tcs.SetResult(null);
        }

        dialog.ShowDialog(MainWindow ?? throw new InvalidOperationException("Main window not found."));

        return tcs.Task;
    }

    public Task<bool> ShowConfirmationDialogAsync(string title, string message)
    {
        var viewModel = new ConfirmationDialogViewModel(title, message);
        var dialog = new DialogWindow
        {
            DataContext = viewModel,
            Width = 350,
            Height = 150
        };

        var tcs = new TaskCompletionSource<bool>();
        viewModel.CloseRequested += confirmed =>
        {
            tcs.SetResult(confirmed);
            dialog.Close();
        };

        dialog.ShowDialog(MainWindow ?? throw new InvalidOperationException("Main window not found."));
        return tcs.Task;
    }
}