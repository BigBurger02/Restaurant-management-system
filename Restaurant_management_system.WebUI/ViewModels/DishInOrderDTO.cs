using System;

namespace Restaurant_management_system.WebUI.ViewModels
{
    public class DishInOrderDTO
    {
        public int DishID { get; set; }
        public string DishName { get; set; } = string.Empty;
        public string TimeOfOrdering { get; set; } = string.Empty;
        public string IsDone { get; set; } = string.Empty;
        public string IsTakenAway { get; set; } = string.Empty;
        public string IsPrioritized { get; set; } = string.Empty;
    }
}

