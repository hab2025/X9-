using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabCo.X9.Core;
using HabCo.X9.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HabCo.X9.App;

public partial class ReportsViewModel : ObservableObject
{
    private readonly AppDbContext _dbContext;
    private readonly IReportService _reportService;

    [ObservableProperty]
    private DateTimeOffset _startDate = DateTimeOffset.Now.AddMonths(-1);

    [ObservableProperty]
    private DateTimeOffset _endDate = DateTimeOffset.Now;

    [ObservableProperty]
    private string? _statusMessage;

    public ReportsViewModel(AppDbContext dbContext, IReportService reportService)
    {
        _dbContext = dbContext;
        _reportService = reportService;
    }

    [RelayCommand]
    private async Task GenerateSalesReportAsync()
    {
        StatusMessage = "Generating report...";

        try
        {
            var bookings = await _dbContext.Bookings
                .Include(b => b.Hall)
                .Where(b => b.Status == BookingStatus.Confirmed &&
                              b.EventDay.Date >= StartDate.Date &&
                              b.EventDay.Date <= EndDate.Date)
                .OrderBy(b => b.EventDay)
                .ToListAsync();

            if (!bookings.Any())
            {
                StatusMessage = "No confirmed bookings found in the selected date range.";
                return;
            }

            var reportBytes = _reportService.CreateSalesPdfReport(bookings, StartDate.DateTime, EndDate.DateTime);

            // Ensure the reports directory exists
            var reportsPath = Path.Combine(AppContext.BaseDirectory, "reports");
            Directory.CreateDirectory(reportsPath);

            var filePath = Path.Combine(reportsPath, $"HABX9_SalesReport_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");

            await File.WriteAllBytesAsync(filePath, reportBytes);

            StatusMessage = $"Report successfully generated and saved to: {filePath}";
        }
        catch (Exception ex)
        {
            StatusMessage = $"An error occurred: {ex.Message}";
        }
    }
}