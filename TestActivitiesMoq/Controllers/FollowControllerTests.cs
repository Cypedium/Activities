using API.Controllers;
using Application.Core;
using Application.Followers;
using Application.Profiles;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestActivitiesMoq.Controllers
{
    public class FollowControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly FollowController _followController;

        public FollowControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _followController = new FollowController(_mediatorMock.Object);
        }

        [Fact]
        public async Task Follow_ShouldReturnExpectedResult()
        {
            // Arrange
            var username = "Bob";
            var expectedResult = new OkResult();
            _mediatorMock.Setup(m => m.Send(It.IsAny<FollowToggle.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<Unit>
                {
                    IsSuccess = true,
                    Value = new Unit()
                });

            // Act
            var result = await _followController.Follow(username);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Unit>(okResult.Value);

            _mediatorMock.Verify(m => m.Send(It.IsAny<FollowToggle.Command>(), It.IsAny<CancellationToken>()),Times.Once);
        }

        [Fact]
        public async Task GetFollowings_ShouldReturnExpectedResult()
        {
            // Arrange
            var username = "Bob";
            var predicate = "testPredicate";
            var expectedResult = new OkResult();
            _mediatorMock.Setup(m => m.Send(It.IsAny<List.Query>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<List<Profile>>()
                {
                    IsSuccess = true,
                    Value = GetSampleProfiles()
                });

            // Act
            var result = await _followController.GetFollowings(username, predicate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Profile>>(okResult.Value);
            Assert.Equal(GetSampleProfiles()[0].Username, returnValue[0].Username);
        }

        private static List<Profile> GetSampleProfiles()
        {
            return new List<Profile>()
            {
                new Profile()
                {
                    Username = "Bob",
                    DisplayName = "Bob",
                    Bio = "",
                    Following = true,
                    FollowersCount = 2,
                    FollowingCount = 3
                }
            };
        }
    }
}
