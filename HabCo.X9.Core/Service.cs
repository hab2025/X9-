using System.Collections.Generic;

namespace HabCo.X9.Core;

public class Service
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }

    public ICollection<BookingService> BookingServices { get; set; } = new List<BookingService>();
}