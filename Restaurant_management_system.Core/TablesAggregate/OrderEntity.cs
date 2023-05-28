using Restaurant_management_system.Core.DishesAggregate;

namespace Restaurant_management_system.Core.TablesAggregate;

public class OrderEntity
{
    public int ID { get; set; }
    public int TableID { get; set; }
    public string? Message { get; set; } = string.Empty;

    public List<DishEntity>? Dishes { get; set; }
}