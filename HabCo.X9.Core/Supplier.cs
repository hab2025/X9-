using System.Collections.Generic;

namespace HabCo.X9.Core;

public class Supplier
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ContactPerson { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    public ICollection<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
}