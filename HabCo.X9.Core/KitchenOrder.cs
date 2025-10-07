using System;
using System.Collections.Generic;

namespace HabCo.X9.Core;

public class KitchenOrder
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public Booking Booking { get; set; } = null!;
    public DateTime IssueDate { get; set; }
    public KitchenOrderStatus Status { get; set; }

    public ICollection<KitchenOrderItem> OrderItems { get; set; } = new List<KitchenOrderItem>();
}