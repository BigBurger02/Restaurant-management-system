namespace Restaurant_management_system.Core.DishesAggregate;

public class DishInOrderEntity
{
    public int ID { get; set; }
    public int OrderID { get; set; }
    public int DishID { get; set; }
    public DateTime DateOfOrdering { get; set; }
    public bool IsDone { get; set; }
    public bool IsTakenAway { get; set; }
    public bool IsPrioritized { get; set; }

    public DishInOrderEntity()
    {
        DateOfOrdering = DateTime.Now;
        IsDone = false;
        IsTakenAway = false;
        IsPrioritized = false;
    }
}