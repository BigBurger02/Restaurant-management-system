using System;
using Restaurant_management_system.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace Restaurant_management_system.WebUI.Controllers
{
    public class TablesController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RestaurantContext _context;

        public TablesController(ILogger<HomeController> logger, RestaurantContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IActionResult Tables()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public IActionResult Menu()
        {
            throw new NotImplementedException();
        }
    }
}

