namespace Restaurant_management_system.Core.TablesAggregate;

public class TableEntity
{
    public int ID { get; set; }
    public bool IsOccupied { get; set; } = false;
    public int AmountOfGuests { get; set; }
    public int OrderCost { get; set; } = 0;
    public bool IsPaid { get; set; } = false;
    public OrderEntity? Order { get; set; }
}