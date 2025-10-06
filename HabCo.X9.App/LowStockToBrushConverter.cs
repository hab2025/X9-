using Avalonia.Data.Converters;
using Avalonia.Media;
using HabCo.X9.Core;
using System;
using System.Globalization;

namespace HabCo.X9.App;

public class LowStockToBrushConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is InventoryItem item)
        {
            if (item.Quantity <= item.ReorderLevel)
            {
                // A light red color to indicate low stock
                return new SolidColorBrush(Colors.MistyRose);
            }
        }
        // Default background
        return Brushes.Transparent;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}