namespace Restaurant_management_system.WebUI.Tests.Api;

public class MenuControllerTests
{
	private XunitLogger<MenuController> _mockLogger;

	public MenuControllerTests(ITestOutputHelper output)
	{
		_mockLogger = new XunitLogger<MenuController>(output);
	}

	[Fact]
	public void GetMenu_ReturnsAllItems()
	{
		// Arrange
		var mockRepo = new Mock<IRestaurantRepository>();
		mockRepo
			.Setup(repo => repo.GetAllDishesFromMenu())
			.Returns(
				new Fixture()
				.CreateMany<DishInMenuEntity>(11)
				.ToList());

		var controller = new MenuController(_mockLogger, mockRepo.Object);

		// Act
		var result = controller.GetMenu();

		// Assert
		Assert.IsType<ObjectResult>(result);
		Assert.NotNull(result.StatusCode);
		Assert.Equal(200, result.StatusCode.Value);
		var value = Assert.IsAssignableFrom<List<DishInMenuEntity>>(result.Value);
		Assert.Equal(mockRepo.Object.GetAllDishesFromMenu().Count(), value.Count());
		Assert.Equivalent(mockRepo.Object.GetAllDishesFromMenu(), value);
	}

	[Fact]
	public void GetMenu_DBTableEmpty_Returns200()
	{
		// Arrange
		var mockRepo = new Mock<IRestaurantRepository>();
		mockRepo
			.Setup(repo => repo.GetAllDishesFromMenu())
			.Returns(new List<DishInMenuEntity>());

		var controller = new MenuController(_mockLogger, mockRepo.Object);

		// Act
		var result = controller.GetMenu();

		// Assert
		Assert.IsType<ObjectResult>(result);
		Assert.NotNull(result.StatusCode);
		Assert.Equal(200, result.StatusCode.Value);
	}

	[Fact]
	public void GetMenu_DBTableReturnedNull_Returns500()
	{
		// Arrange
		List<DishInMenuEntity> items = null!;
		var mockRepo = new Mock<IRestaurantRepository>();
		mockRepo
			.Setup(repo => repo.GetAllDishesFromMenu())
			.Returns(items);

		var controller = new MenuController(_mockLogger, mockRepo.Object);

		// Act
		var result = controller.GetMenu();

		// Assert
		Assert.IsType<ObjectResult>(result);
		Assert.NotNull(result.StatusCode);
		Assert.Equal(500, result.StatusCode.Value);
	}
}