namespace Restaurant_management_system.WebUI.Tests.Api;

public class OrderControllerTests
{
	private XunitLogger<OrderController> _mockLogger;

	public OrderControllerTests(ITestOutputHelper output)
	{
		_mockLogger = new XunitLogger<OrderController>(output);
	}

	[Theory]
	[InlineData(3)]
	[InlineData(4)]
	public void CreateOrder_CreatesOrderForGivenTable(int tableID)
	{
		// Arrange
		var mockRepo = new Mock<IRestaurantRepository>();
		mockRepo
			.Setup(repo => repo.FindTableByID(tableID))
			.Returns(new Fixture().Create<TableEntity>());
		mockRepo
			.Setup(repo => repo.CreateOrder(tableID))
			.Returns(new Fixture()
				.Build<OrderInTableEntity>()
				.With(e => e.TableID, tableID)
				.Create());

		var controller = new OrderController(_mockLogger, mockRepo.Object);

		// Act
		var result = controller.CreateOrder(tableID);

		// Assert
		Assert.IsType<CreatedResult>(result);
		Assert.NotNull(result.StatusCode);
		Assert.Equal(201, result.StatusCode.Value);
		var value = Assert.IsAssignableFrom<int>(result.Value);
		Assert.Equivalent(mockRepo.Object.CreateOrder(tableID).ID, value);
	}

	[Theory]
	[InlineData(3)]
	[InlineData(4)]
	public void CreateOrder_NonExistentTable_404(int tableID)
	{
		// Arrange
		var mockRepo = new Mock<IRestaurantRepository>();
		TableEntity table = null!;
		mockRepo
			.Setup(repo => repo.FindTableByID(tableID))
			.Returns(table);
		mockRepo
			.Setup(repo => repo.CreateOrder(tableID))
			.Returns(new Fixture()
				.Build<OrderInTableEntity>()
				.With(e => e.TableID, tableID)
				.Create());

		var controller = new OrderController(_mockLogger, mockRepo.Object);

		// Act
		var result = controller.CreateOrder(3);

		Assert.IsType<NotFoundObjectResult>(result);
	}

	[Theory]
	[InlineData(3)]
	[InlineData(4)]
	public void CloseOrder_SetOpenPropertyToFalse_ReturnsTrue(int orderID)
	{
		// Arrange
		var mockRepo = new Mock<IRestaurantRepository>();
		mockRepo
			.Setup(repo => repo.CloseOrderByID(orderID))
			.Returns(true);

		var controller = new OrderController(_mockLogger, mockRepo.Object);

		// Act
		var result = controller.CloseOrder(orderID);

		// Assert
		Assert.IsType<NoContentResult>(result);
	}

	[Theory]
	[InlineData(3)]
	[InlineData(4)]
	public void CloseOrder_SetOpenPropertyToFalse_ReturnsFalse(int orderID)
	{
		// Arrange
		var mockRepo = new Mock<IRestaurantRepository>();
		mockRepo
			.Setup(repo => repo.CloseOrderByID(orderID))
			.Returns(false);

		var controller = new OrderController(_mockLogger, mockRepo.Object);

		// Act
		var result = controller.CloseOrder(orderID);

		// Assert
		Assert.IsType<BadRequestResult>(result);
	}
}