using API.Controllers;
using Application.Activities;
using Autofac.Extras.Moq;

namespace MoqProjectTests.Controllers
{
    public class ActivitiesControllerTests
    {
        //[Fact]
        //public void Get_Activities_ValidCall()
        //{
        //    using var mock = AutoMock.GetLoose();
        //    ActivityParams testParam = new()
        //    {
        //        IsGoing = true,
        //        IsHost = false,
        //        StartDate = DateTime.UtcNow
        //    };

        //    var controller = new ActivitiesController();
        //    controller.Request = new HttpRequestMessage();
        //    controller.Configuration = new HttpConfiguration();

        //    var result = controller.GetActivities(testParam);

        //    var cls = mock.Create<ActivitiesController>();

        //    var actual = cls.GetActivities(testParam);

        //    Assert.NotNull(result);
        //    Assert.Equal(1, actual.Id);
        //}

        [Fact]
        public async Task GetActivities_ReturnsExpectedResult()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var controller = new ActivitiesController(mediatorMock.Object);
            var activityParams = new ActivityParams();

            var expectedResult = new IActionResult(); // replace with your expected result
            mediatorMock.Setup(m => m.Send(It.IsAny<List.Query>())).ReturnsAsync(expectedResult);

            // Act
            var result = await controller.GetActivities(activityParams);

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
