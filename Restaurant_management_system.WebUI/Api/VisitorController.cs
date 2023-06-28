using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

using Restaurant_management_system.WebUI.ApiModels;
using Restaurant_management_system.Infrastructure.Data;
using Restaurant_management_system.Core.DishesAggregate;

namespace Restaurant_management_system.WebUI.Api;

[AllowAnonymous]
[Route("api/[controller]")]
public class VisitorController : Controller
{
    private readonly ILogger<VisitorController> _logger;
    private readonly RestaurantContext _context;
    private readonly IStringLocalizer<VisitorController> _localizer;

    public VisitorController(ILogger<VisitorController> logger, RestaurantContext context, IStringLocalizer<VisitorController> localizer)
    {
        _logger = logger;
        _context = context;
        _localizer = localizer;
    }

    // GET: api/values
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






    //// GET: api/values
    //[HttpGet]
    //public IEnumerable<string> Get()
    //{
    //    return new string[] { "value1", "value2" };
    //}

    // GET api/values/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/values
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}

