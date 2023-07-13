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
		var data = new DataGenerator();
		data.GenerateBogusData();
		mockRepo
			.Setup(repo => repo.GetAllDishesFromMenu())
			.Returns(data.DishesInMenu);

		var controller = new MenuController(_mockLogger, mockRepo.Object);

		// Act
		var result = controller.GetMenu();

		// Assert
		result.Should().BeOfType<ObjectResult>();
		result.StatusCode.Should().NotBeNull();
		result.StatusCode!.Value.Should().Be(200);
		result.Value.Should().BeOfType<List<DishInMenuEntity>>().And.NotBeNull();
		List<DishInMenuEntity> list = (List<DishInMenuEntity>)result.Value!;
		list.Count().Should().Be(mockRepo.Object.GetAllDishesFromMenu().Count());
		list.Should().BeEquivalentTo(mockRepo.Object.GetAllDishesFromMenu());
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
		result.Should().BeOfType<ObjectResult>();
		result.StatusCode.Should().NotBeNull();
		result.StatusCode!.Value.Should().Be(200);
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
		result.Should().BeOfType<ObjectResult>();
		result.StatusCode.Should().NotBeNull();
		result.StatusCode!.Value.Should().Be(500);
	}
}