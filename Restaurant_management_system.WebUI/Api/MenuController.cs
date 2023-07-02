﻿using Microsoft.AspNetCore.Mvc;
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
    /// Get all dishes
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     GET api/menu
    /// </remarks>
    /// <response code="200">Returns all menu items</response>
    /// <response code="204">Menu is empty</response>
    // GET: api/menu
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(204)]
    [Produces("application/json")]
    public ActionResult<List<DishItemDTO>> GetMenu()
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

        if (dishes.Count == 0)
            return NoContent();

        // Ingredients:
        foreach (var oneMenuEntity in dishes)
        {
            var ingredientsID = _context.IngredientForDishInMenu
                .AsNoTracking()
                .Where(i => i.DishInMenuID == oneMenuEntity.ID);

            foreach (var oneMenuInredientsEntity in ingredientsID.ToList())
            {
                oneMenuEntity.Ingredients.Add(_context.Ingredient
                    .AsNoTracking()
                    .FirstOrDefault(i => i.ID == oneMenuInredientsEntity.IngredientID).Name);
            }
        }

        return Ok(dishes);
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