using System;

namespace Restaurant_management_system.WebUI.ViewModels
{
    public class DishInOrderDTO
    {
        public int DishID { get; set; }
        public int TableID { get; set; }
        public int OrderID { get; set; }
        public int DishInMenuID { get; set; }
        public string DishName { get; set; } = string.Empty;
        public string TimeOfOrderingString { get; set; } = string.Empty;
        public string IsDoneString { get; set; } = string.Empty;
        public string IsTakenAwayString { get; set; } = string.Empty;
        public string IsPrioritizedString { get; set; } = string.Empty;
        public bool IsDoneBool { get; set; }
        public bool IsTakenAwayBool { get; set; }
        public bool IsPrioritizedBool { get; set; }

        public List<DishInMenuDTO> DishDTOs { get; set; } = new List<DishInMenuDTO>();
    }
}

