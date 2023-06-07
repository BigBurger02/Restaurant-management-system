using System;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_management_system.WebUI.ViewModels;

public class TableDTO
{
    [Key]
    public int ID { get; set; }
    public string IsOccupiedString { get; set; } = string.Empty;
    public string IsPaidString { get; set; } = string.Empty;
    public bool IsOccupiedBool { get; set; } = false;
    public bool IsPaidBool { get; set; } = false;
    [Required]
    [Range(0, 1000000, ErrorMessage = "Amount of guests must be between 0 and 1000000")]
    public int AmountOfGuests { get; set; } = 0;
    public int OrderCost { get; set; } = 0;

    public OrderInTableDTO? Order { get; set; }
}

