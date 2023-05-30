using Restaurant_management_system.Core.DishesAggregate;

namespace Restaurant_management_system.Core.TablesAggregate;

public class OrderEntity
{
    public int ID { get; set; }
    public int TableID { get; set; }
    public bool Open { get; set; }
    public string? Message { get; set; }

    public List<DishEntity>? Dishes { get; set; }

    public OrderEntity()
    {
        Open = true;
        Message = string.Empty;

        Dishes = new List<DishEntity>();
    }
    public OrderEntity(int id)
    {
        TableID = id;
        Open = true;
        Message = string.Empty;

        Dishes = new List<DishEntity>();
    }
}