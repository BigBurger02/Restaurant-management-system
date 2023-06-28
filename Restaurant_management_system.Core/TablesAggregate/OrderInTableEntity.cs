using Restaurant_management_system.Core.DishesAggregate;

namespace Restaurant_management_system.Core.TablesAggregate;

public class OrderInTableEntity
{
    public int ID { get; set; }
    public int TableID { get; set; }
    public bool Open { get; set; }
    public bool SelfOrdered { get; set; }
    public string? Message { get; set; }

    public List<DishInOrderEntity>? Dishes { get; set; }

    public OrderInTableEntity()
    {
        Open = true;
        SelfOrdered = false;
        Message = string.Empty;

        Dishes = new List<DishInOrderEntity>();
    }
    public OrderInTableEntity(int id)
    {
        TableID = id;
        Open = true;
        SelfOrdered = false;
        Message = string.Empty;

        Dishes = new List<DishInOrderEntity>();
    }
}