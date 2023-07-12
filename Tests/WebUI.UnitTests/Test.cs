using Restaurant_management_system.WebUI;
using Restaurant_management_system.Infrastructure.Data;
using Restaurant_management_system.Core.DishesAggregate;
using Restaurant_management_system.Core.TablesAggregate;
using AutoFixture;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;




namespace WebUI.UnitTests;

public class EmployeeFullNameTests
{
	[Fact]
	public async Task Test()
	{
		// Arrange
		var mockRepo = new Mock<RestaurantContext>();
		mockRepo.Setup(repo => repo.Ingredient).Returns(GetTestItems());
	}

	private List<IngredientEntity> GetTestItems()
	{
		var ingredients = new List<IngredientEntity>()
		{
			new IngredientEntity()
			{
				ID = 1,
				Name = "Potato",
				Price = 100
			},
			new IngredientEntity()
			{
				ID = 2,
				Name = "Salt",
				Price = 10
			},
			new IngredientEntity()
			{
				ID = 3,
				Name = "Sugar",
				Price = 10
			},
			new IngredientEntity()
			{
				ID = 4,
				Name = "Cheese",
				Price = 10
			},
			new IngredientEntity()
			{
				ID = 5,
				Name = "Bread",
				Price = 10
			}
		};


		return ingredients;
	}
}