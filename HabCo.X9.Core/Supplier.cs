using System.Collections.Generic;

namespace HabCo.X9.Core;

public class Supplier
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ContactPerson { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }

    public ICollection<InventoryItem> InventoryItems { get; set; }
}