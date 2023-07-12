using System;
using Microsoft.EntityFrameworkCore;
using Restaurant_management_system.Core.DishesAggregate;

namespace WebUI.UnitTests
{
	public class EmptyClass
	{
		public DbSet<IngredientEntity> Ingredient { get; set; }

		public EmptyClass()
		{
		}
	}
}

