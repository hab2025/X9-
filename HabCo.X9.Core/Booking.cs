using System;

namespace HabCo.X9.Core;

public class Booking
{
    public int Id { get; set; }
    public string ClientName { get; set; }
    public string ClientPhone { get; set; }
    public string ClientEmail { get; set; }
    public DateTime EventDay { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int HallId { get; set; }
    public Hall Hall { get; set; }
    public decimal TotalCost { get; set; }
    public BookingStatus Status { get; set; }
}