namespace Restaurant_management_system.Core.TablesAggregate;

public class TableEntity
{
    public int ID { get; set; }
    public bool IsOccupied { get; set; }
    public int AmountOfGuests { get; set; }
    public int OrderCost { get; set; }
    public bool IsPaid { get; set; }
    public OrderEntity? Order { get; set; }

    public TableEntity()
    {
        IsOccupied = false;
        AmountOfGuests = 0;
        OrderCost = 0;
        IsPaid = false;

        Order = new OrderEntity();
    }
}