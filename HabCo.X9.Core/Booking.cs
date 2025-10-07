using System;

namespace HabCo.X9.Core;

public class Booking
{
    public int Id { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string ClientPhone { get; set; } = string.Empty;
    public string ClientEmail { get; set; } = string.Empty;
    public DateTime EventDay { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int HallId { get; set; }
    public Hall Hall { get; set; } = null!;
    public decimal TotalCost { get; set; }
    public DiscountType DiscountType { get; set; }
    public decimal DiscountValue { get; set; }
    public BookingStatus Status { get; set; }
}