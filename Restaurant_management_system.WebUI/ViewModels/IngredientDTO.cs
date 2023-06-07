using System;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_management_system.WebUI.ViewModels;

public class IngredientDTO
{
    [Key]
    public int? ID { get; set; }
    [Required]
    [MinLength(1, ErrorMessage = "Name can't be less than 1 symbols")]
    [MaxLength(30, ErrorMessage = "Name can't be mo than 30 symbols")]
    public string Name { get; set; } = string.Empty;
    [Range(1, 1000000, ErrorMessage = "Price must be between 1 and 1000000")]
    public int Price { get; set; }
}

