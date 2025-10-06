namespace HabCo.X9.Core;

public class InventoryItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public string Unit { get; set; } // e.g., "kg", "bottle", "box"
    public int ReorderLevel { get; set; } // Alert when quantity falls to this level

    public int? SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
}