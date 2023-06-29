using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

using Restaurant_management_system.WebUI.ApiModels;
using Restaurant_management_system.Infrastructure.Data;
using Restaurant_management_system.Core.DishesAggregate;
using Restaurant_management_system.Core.TablesAggregate;

namespace Restaurant_management_system.WebUI.Api;

[AllowAnonymous]
[Route("api/[controller]")]
public class MenuController : Controller
{
    private readonly ILogger<MenuController> _logger;
    private readonly RestaurantContext _context;
    private readonly IStringLocalizer<MenuController> _localizer;

    public MenuController(ILogger<MenuController> logger, RestaurantContext context, IStringLocalizer<MenuController> localizer)
    {
        _logger = logger;
        _context = context;
        _localizer = localizer;
    }

    // GET: api/visitor
    [HttpGet]
    public List<DishItemDTO> GetMenu()
    {
        var dishes = _context.DishInMenu
            .AsNoTracking()
            .Select(d => new DishItemDTO
            {
                ID = d.ID,
                Name = d.Name,
                Price = d.Price
            })
            .ToList();

        // Ingredients:
        foreach (var oneMenuEntity in dishes)
        {
            var ingredientsID = _context.IngredientForDishInMenu
                .AsNoTracking()
                .Where(i => i.DishInMenuID == oneMenuEntity.ID);

            foreach (var oneMenuInredientsEntity in ingredientsID)
            {
                oneMenuEntity.Ingredients.Add(_context.Ingredient
                    .AsNoTracking()
                    .FirstOrDefault(i => i.ID == oneMenuInredientsEntity.IngredientID).Name);
            }
        }

        return dishes;
    }
}