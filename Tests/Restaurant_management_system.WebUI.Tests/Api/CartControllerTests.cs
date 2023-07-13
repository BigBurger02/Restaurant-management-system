namespace Restaurant_management_system.WebUI.Tests.Api;

public class CartControllerTests
{
	private XunitLogger<CartController> _mockLogger;

	public CartControllerTests(ITestOutputHelper output)
	{
		_mockLogger = new XunitLogger<CartController>(output);
	}

	[Theory]
	[InlineData(3, 5)]
	[InlineData(4, 8)]
	public void AddToCart_Returns204(int orderID, int dishIdInMenu)
	{
		// Arrange
		var mockRepo = new Mock<IRestaurantRepository>();
		mockRepo
			.Setup(repo => repo.FindOrderByID(orderID))
			.Returns(new Fixture()
				.Build<OrderInTableEntity>()
				.With(e => e.ID, orderID)
				.With(e => e.SelfOrdered, true)
				.Create());
		mockRepo
			.Setup(repo => repo.FindDishInMenuById(dishIdInMenu))
			.Returns(new Fixture()
				.Build<DishInMenuEntity>()
				.With(e => e.ID, dishIdInMenu)
				.Create());
		mockRepo
			.Setup(repo => repo.CreateDishInOrder(orderID, dishIdInMenu))
			.Returns(new Fixture()
				.Build<DishInOrderEntity>()
				.With(e => e.OrderID, orderID)
				.With(e => e.DishID, dishIdInMenu)
				.With(e => e.IsDone, false)
				.With(e => e.IsTakenAway, false)
				.Create());

		var controller = new CartController(_mockLogger, mockRepo.Object);

		// Act
		var result = controller.AddToCart(orderID, dishIdInMenu);

		// Assert
		Assert.IsType<ObjectResult>(result);
		Assert.NotNull(result.StatusCode);
		Assert.Equal(204, result.StatusCode.Value);
	}

	[Theory]
	[InlineData(3, 5)]
	[InlineData(4, 8)]
	public void AddToCart_OrderNotFound_404(int orderID, int dishIdInMenu)
	{
		// Arrange
		var mockRepo = new Mock<IRestaurantRepository>();
		OrderInTableEntity order = null!;
		mockRepo
			.Setup(repo => repo.FindOrderByID(orderID))
			.Returns(order!);
		mockRepo
			.Setup(repo => repo.FindDishInMenuById(dishIdInMenu))
			.Returns(new Fixture()
				.Build<DishInMenuEntity>()
				.With(e => e.ID, dishIdInMenu)
				.Create());
		mockRepo
			.Setup(repo => repo.CreateDishInOrder(orderID, dishIdInMenu))
			.Returns(new Fixture()
				.Build<DishInOrderEntity>()
				.With(e => e.OrderID, orderID)
				.With(e => e.DishID, dishIdInMenu)
				.With(e => e.IsDone, false)
				.With(e => e.IsTakenAway, false)
				.Create());

		var controller = new CartController(_mockLogger, mockRepo.Object);

		// Act
		var result = controller.AddToCart(orderID, dishIdInMenu);

		// Assert
		Assert.IsType<NotFoundObjectResult>(result);
	}

	[Theory]
	[InlineData(3, 5)]
	[InlineData(4, 8)]
	public void AddToCart_DishNotFound_404(int orderID, int dishIdInMenu)
	{
		// Arrange
		var mockRepo = new Mock<IRestaurantRepository>();
		mockRepo
			.Setup(repo => repo.FindOrderByID(orderID))
			.Returns(new Fixture()
				.Build<OrderInTableEntity>()
				.With(e => e.ID, orderID)
				.With(e => e.SelfOrdered, true)
				.Create());
		DishInMenuEntity dish = null!;
		mockRepo
			.Setup(repo => repo.FindDishInMenuById(dishIdInMenu))
			.Returns(dish!);
		mockRepo
			.Setup(repo => repo.CreateDishInOrder(orderID, dishIdInMenu))
			.Returns(new Fixture()
				.Build<DishInOrderEntity>()
				.With(e => e.OrderID, orderID)
				.With(e => e.DishID, dishIdInMenu)
				.With(e => e.IsDone, false)
				.With(e => e.IsTakenAway, false)
				.Create());

		var controller = new CartController(_mockLogger, mockRepo.Object);

		// Act
		var result = controller.AddToCart(orderID, dishIdInMenu);

		// Assert
		Assert.IsType<NotFoundObjectResult>(result);
	}

	[Theory]
	[InlineData(3, 5)]
	[InlineData(4, 8)]
	public void AddToCart_DishNotCreatedError_CreateDishInOrderReturnsNULL_500(int orderID, int dishIdInMenu)
	{
		// Arrange
		var mockRepo = new Mock<IRestaurantRepository>();
		mockRepo
			.Setup(repo => repo.FindOrderByID(orderID))
			.Returns(new Fixture()
				.Build<OrderInTableEntity>()
				.With(e => e.ID, orderID)
				.With(e => e.SelfOrdered, true)
				.Create());
		mockRepo
			.Setup(repo => repo.FindDishInMenuById(dishIdInMenu))
			.Returns(new Fixture()
				.Build<DishInMenuEntity>()
				.With(e => e.ID, dishIdInMenu)
				.Create());
		DishInOrderEntity dish = null!;
		mockRepo
			.Setup(repo => repo.CreateDishInOrder(orderID, dishIdInMenu))
			.Returns(dish!);

		var controller = new CartController(_mockLogger, mockRepo.Object);

		// Act
		var result = controller.AddToCart(orderID, dishIdInMenu);

		// Assert
		Assert.IsType<ObjectResult>(result);
		Assert.NotNull(result.StatusCode);
		Assert.Equal(500, result.StatusCode.Value);
	}

	[Theory]
	[InlineData(3, 5)]
	[InlineData(4, 8)]
	public void AddToCart_DishNotCreatedError_CreateDishInOrderReturnsID0_500(int orderID, int dishIdInMenu)
	{
		// Arrange
		var mockRepo = new Mock<IRestaurantRepository>();
		mockRepo
			.Setup(repo => repo.FindOrderByID(orderID))
			.Returns(new Fixture()
				.Build<OrderInTableEntity>()
				.With(e => e.ID, orderID)
				.With(e => e.SelfOrdered, true)
				.Create());
		mockRepo
			.Setup(repo => repo.FindDishInMenuById(dishIdInMenu))
			.Returns(new Fixture()
				.Build<DishInMenuEntity>()
				.With(e => e.ID, dishIdInMenu)
				.Create());
		DishInOrderEntity dish = new DishInOrderEntity() { ID = 0 };
		mockRepo
			.Setup(repo => repo.CreateDishInOrder(orderID, dishIdInMenu))
			.Returns(dish!);

		var controller = new CartController(_mockLogger, mockRepo.Object);

		// Act
		var result = controller.AddToCart(orderID, dishIdInMenu);

		// Assert
		Assert.IsType<ObjectResult>(result);
		Assert.NotNull(result.StatusCode);
		Assert.Equal(500, result.StatusCode.Value);
	}

	[Theory]
	[InlineData(3, 5)]
	[InlineData(4, 8)]
	public void DeleteFromCart_Returns204(int orderID, int dishIdInMenu)
	{
		// Arrange
		var mockRepo = new Mock<IRestaurantRepository>();
		mockRepo
			.Setup(repo => repo.FindOrderByID(orderID))
			.Returns(new Fixture()
				.Build<OrderInTableEntity>()
				.With(e => e.ID, orderID)
				.With(e => e.SelfOrdered, true)
				.Create());
		mockRepo
			.Setup(repo => repo.FindDishInMenuById(dishIdInMenu))
			.Returns(new Fixture()
				.Build<DishInMenuEntity>()
				.With(e => e.ID, dishIdInMenu)
				.Create());
		mockRepo
			.Setup(repo => repo.FindDishInOrderByIdInMenu(orderID, dishIdInMenu))
			.Returns(new Fixture()
				.Build<DishInOrderEntity>()
				.With(e => e.OrderID, orderID)
				.With(e => e.DishID, dishIdInMenu)
				.With(e => e.IsDone, false)
				.With(e => e.IsTakenAway, false)
				.Create());
		mockRepo
			.Setup(repo => repo.RemoveDishFromOrderByID(orderID, dishIdInMenu))
			.Returns(true);

		var controller = new CartController(_mockLogger, mockRepo.Object);

		// Act
		var result = controller.DeleteFromCart(orderID, dishIdInMenu);

		// Assert
		Assert.IsType<ObjectResult>(result);
		Assert.NotNull(result.StatusCode);
		Assert.Equal(204, result.StatusCode.Value);
	}

	[Theory]
	[InlineData(3, 5)]
	[InlineData(4, 8)]
	public void DeleteFromCart_OrderNotFound_404(int orderID, int dishIdInMenu)
	{
		// Arrange
		var mockRepo = new Mock<IRestaurantRepository>();
		OrderInTableEntity order = null!;
		mockRepo
			.Setup(repo => repo.FindOrderByID(orderID))
			.Returns(order!);
		mockRepo
			.Setup(repo => repo.FindDishInMenuById(dishIdInMenu))
			.Returns(new Fixture()
				.Build<DishInMenuEntity>()
				.With(e => e.ID, dishIdInMenu)
				.Create());
		mockRepo
			.Setup(repo => repo.FindDishInOrderByIdInMenu(orderID, dishIdInMenu))
			.Returns(new Fixture()
				.Build<DishInOrderEntity>()
				.With(e => e.OrderID, orderID)
				.With(e => e.DishID, dishIdInMenu)
				.With(e => e.IsDone, false)
				.With(e => e.IsTakenAway, false)
				.Create());
		mockRepo
			.Setup(repo => repo.RemoveDishFromOrderByID(orderID, dishIdInMenu))
			.Returns(true);

		var controller = new CartController(_mockLogger, mockRepo.Object);

		// Act
		var result = controller.DeleteFromCart(orderID, dishIdInMenu);

		// Assert
		Assert.IsType<NotFoundObjectResult>(result);
	}

	[Theory]
	[InlineData(3, 5)]
	[InlineData(4, 8)]
	public void DeleteFromCart_DishInMenuNotFound_404(int orderID, int dishIdInMenu)
	{
		// Arrange
		var mockRepo = new Mock<IRestaurantRepository>();
		mockRepo
			.Setup(repo => repo.FindOrderByID(orderID))
			.Returns(new Fixture()
				.Build<OrderInTableEntity>()
				.With(e => e.ID, orderID)
				.With(e => e.SelfOrdered, true)
				.Create());
		DishInMenuEntity dish = null!;
		mockRepo
			.Setup(repo => repo.FindDishInMenuById(dishIdInMenu))
			.Returns(dish!);
		mockRepo
			.Setup(repo => repo.FindDishInOrderByIdInMenu(orderID, dishIdInMenu))
			.Returns(new Fixture()
				.Build<DishInOrderEntity>()
				.With(e => e.OrderID, orderID)
				.With(e => e.DishID, dishIdInMenu)
				.With(e => e.IsDone, false)
				.With(e => e.IsTakenAway, false)
				.Create());
		mockRepo
			.Setup(repo => repo.RemoveDishFromOrderByID(orderID, dishIdInMenu))
			.Returns(true);

		var controller = new CartController(_mockLogger, mockRepo.Object);

		// Act
		var result = controller.DeleteFromCart(orderID, dishIdInMenu);

		// Assert
		Assert.IsType<NotFoundObjectResult>(result);
	}

	[Theory]
	[InlineData(3, 5)]
	[InlineData(4, 8)]
	public void DeleteFromCart_DishInOrderNotFound_404(int orderID, int dishIdInMenu)
	{
		// Arrange
		var mockRepo = new Mock<IRestaurantRepository>();
		mockRepo
			.Setup(repo => repo.FindOrderByID(orderID))
			.Returns(new Fixture()
				.Build<OrderInTableEntity>()
				.With(e => e.ID, orderID)
				.With(e => e.SelfOrdered, true)
				.Create());
		mockRepo
			.Setup(repo => repo.FindDishInMenuById(dishIdInMenu))
			.Returns(new Fixture()
				.Build<DishInMenuEntity>()
				.With(e => e.ID, dishIdInMenu)
				.Create());
		DishInOrderEntity dish = null!;
		mockRepo
			.Setup(repo => repo.FindDishInOrderByIdInMenu(orderID, dishIdInMenu))
			.Returns(dish!);
		mockRepo
			.Setup(repo => repo.RemoveDishFromOrderByID(orderID, dishIdInMenu))
			.Returns(true);

		var controller = new CartController(_mockLogger, mockRepo.Object);

		// Act
		var result = controller.DeleteFromCart(orderID, dishIdInMenu);

		// Assert
		Assert.IsType<NotFoundObjectResult>(result);
	}
}