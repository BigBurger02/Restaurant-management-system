using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

using Restaurant_management_system.WebUI.ApiModels;
using Restaurant_management_system.Infrastructure.Data;
using Restaurant_management_system.Core.DishesAggregate;
using Restaurant_management_system.Core.TablesAggregate;
using Restaurant_management_system.Core.Services.Attributes;

namespace Restaurant_management_system.WebUI.Api;

[AllowAnonymous]
[Route("api/[controller]")]
[SwaggerControllerOrder(1)]
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

    /// <summary>
    /// Take all dishes (in development)
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     GET api/menu
    /// </remarks>
    /// <response code="201">Returns all menu items</response>
    /// <response code="400">Some problems (not implemented)</response>
    // GET: api/menu
    [HttpGet]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [Produces("application/json")]
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

//// <remarks>
//// Sample request:
//// 
////     GET api/menu
////     {
////       "firstName": "Mike",
////       "lastName": "Andrew",
////       "emailId": "Mike.Andrew@gmail.com"
////     }
//// </ remarks >