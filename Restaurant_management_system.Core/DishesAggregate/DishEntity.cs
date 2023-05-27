namespace Restaurant_management_system.Core.DishesAggregate
{
    public class DishEntity
    {
        public int ID { get; set; }
        public string DishName { get; set; } = string.Empty;
        public string DateOfOrdering { get; set; } = string.Empty;
        public bool IsDone { get; set; }
        public bool IsTakenAway { get; set; }
        public bool IsPrioritized { get; set; }
    }
}

