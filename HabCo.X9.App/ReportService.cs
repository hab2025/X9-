using HabCo.X9.Core;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HabCo.X9.App;

public class ReportService : IReportService
{
    public byte[] CreateSalesPdfReport(List<Booking> bookings, DateTime startDate, DateTime endDate)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .Text($"Sales Report - {startDate:d} to {endDate:d}")
                    .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(x =>
                    {
                        x.Spacing(20);

                        x.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Event Date");
                                header.Cell().Text("Client");
                                header.Cell().Text("Hall");
                                header.Cell().AlignRight().Text("Cost");
                            });

                            foreach (var booking in bookings)
                            {
                                table.Cell().Text($"{booking.EventDay:d}");
                                table.Cell().Text(booking.ClientName);
                                table.Cell().Text(booking.Hall.Name);
                                table.Cell().AlignRight().Text($"{booking.TotalCost:C}");
                            }
                        });

                        var totalRevenue = bookings.Sum(b => b.TotalCost);
                        x.Item().AlignRight().Text($"Total Revenue: {totalRevenue:C}").SemiBold().FontSize(14);
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                    });
            });
        }).GeneratePdf();
    }
}