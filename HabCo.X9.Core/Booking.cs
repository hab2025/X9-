using System;

namespace HabCo.X9.Core;

public class Booking
{
    public int Id { get; set; }
    public string ClientName { get; set; }
    public string ClientPhone { get; set; }
    public string ClientEmail { get; set; }
    public DateTime EventDate { get; set; }
    public int HallId { get; set; }
    public Hall Hall { get; set; }
    public decimal TotalCost { get; set; }
    public bool IsConfirmed { get; set; }
}