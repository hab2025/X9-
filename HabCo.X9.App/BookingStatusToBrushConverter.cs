using Avalonia.Data.Converters;
using Avalonia.Media;
using HabCo.X9.Core;
using System;
using System.Globalization;

namespace HabCo.X9.App;

public class BookingStatusToBrushConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is BookingStatus status)
        {
            return status switch
            {
                BookingStatus.Confirmed => Brushes.Green,
                BookingStatus.Pending => Brushes.Orange,
                BookingStatus.Cancelled => Brushes.Red,
                _ => Brushes.Gray
            };
        }
        return Brushes.Gray;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}