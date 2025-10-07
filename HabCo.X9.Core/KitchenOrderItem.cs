namespace HabCo.X9.Core;

public class KitchenOrderItem
{
    public int Id { get; set; }

    public int KitchenOrderId { get; set; }
    public KitchenOrder KitchenOrder { get; set; } = null!;

    public int InventoryItemId { get; set; }
    public InventoryItem InventoryItem { get; set; } = null!;

    public int Quantity { get; set; }
}