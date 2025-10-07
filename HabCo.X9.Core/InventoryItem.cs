namespace HabCo.X9.Core;

public class InventoryItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string Unit { get; set; } = string.Empty; // e.g., "kg", "bottle", "box"
    public int ReorderLevel { get; set; } // Alert when quantity falls to this level

    public int? SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
}