using API.Controllers;
using Application.Activities;
using Application.Core;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace TestActivitiesMoq.Controllers
{
    public class ActivitiesControllerTests : BaseApiController
    {
        [Fact]
        public async Task GetActivitiesReturnsExpectedResult()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();

            var items = GetSampleListActivityDto();
            var count = 1;
            var pageNumber = 1;
            var pageSize = 10;

            var expectedPagedResult = new PagedList<ActivityDto>(items, count, pageNumber, pageSize);

            var param = new ActivityParams()
            {
                IsGoing = false,
                IsHost = false,
                StartDate = DateTime.UtcNow,
            };

            var queryParam = new List.Query { Params = param };

            mockMediator
                .Setup(m => m.Send(It.IsAny<ActivityParams>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedPagedResult);

            var sut = new ActivitiesController(mockMediator.Object);

            //// Act
            var result = await sut.GetActivities(param);

            mockMediator.Verify(x => x.Send(It.IsAny<List.Query>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GetActivity_ReturnsCorrectResult()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var id = Guid.NewGuid();

            mockMediator.Setup(m => m.Send(It.IsAny<Details.Query>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<ActivityDto>
                {
                    IsSuccess = true,
                    Value = GetSampleListActivityDto()[0] // Set up your object here
                });

            // Act
            var sut = new ActivitiesController(mockMediator.Object);
            var result = await sut.GetActivity(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ActivityDto>(okResult.Value);

            mockMediator.Verify(x => x.Send(It.IsAny<Details.Query>(), It.IsAny<CancellationToken>()), Times.Once());

            Assert.Equal(GetSampleListActivityDto()[0].Title, returnValue.Title);
        }

        [Fact]
        public async Task CreateActivity_ReturnsCorrectResult()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var activity = GetSampleActivity();

            mockMediator.Setup(m => m.Send(It.IsAny<Create.Command>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<Unit>
            {
                IsSuccess = true,
                Value = new Unit()
            });

            // Act
            var sut = new ActivitiesController(mockMediator.Object);
            var result = await sut.CreateActivity(activity);

            // Assert
            var unitResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<OkObjectResult>(unitResult);

            mockMediator.Verify(x => x.Send(It.IsAny<Create.Command>(), It.IsAny<CancellationToken>()), Times.Exactly(1));
        }

        [Fact]
        public async Task EditActivity_ReturnsCorrectResult()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var sut = new ActivitiesController(mockMediator.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = SetupDefaultContextWithResponseBodyStream(),
                }
            };

            var activity = GetSampleActivity();

            activity.City = "TestCity";

            mockMediator.Setup(m => m.Send(It.IsAny<Edit.Command>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<Unit>
            {
                IsSuccess = true,
                Value = new Unit()
            });

            // Act
            var result = await sut.EditActivity(activity.Id, activity);

            // Assert
            var unitResult = Assert.IsType<OkObjectResult>(result);

            mockMediator.Verify(x => x.Send(It.IsAny<Edit.Command>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteActivity_ReturnsCorrectResult()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var sut = new ActivitiesController(mockMediator.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = SetupDefaultContextWithResponseBodyStream(),
                }
            };

            var activity = GetSampleActivity();

            mockMediator.Setup(m => m.Send(It.IsAny<Delete.Command>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<Unit>
            {
                IsSuccess = true,
                Value = new Unit()
            });

            // Act
            var result = await sut.DeleteActivity(activity.Id);

            // Assert
            var unitResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<OkObjectResult>(unitResult);

            mockMediator.Verify(x => x.Send(It.IsAny<Delete.Command>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task UpdateActivityAttendee_ReturnsCorrectResult()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var sut = new ActivitiesController(mockMediator.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = SetupDefaultContextWithResponseBodyStream(),
                }
            };

            var activity = GetSampleActivity();

            mockMediator.Setup(m => m.Send(It.IsAny<UpdateAttendence.Command>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<Unit>
            {
                IsSuccess = true,
                Value = new Unit()
            });

            // Act
            var result = await sut.Attend(activity.Id);

            // Assert
            var unitResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<OkObjectResult>(unitResult);

            mockMediator.Verify(x => x.Send(It.IsAny<UpdateAttendence.Command>(), It.IsAny<CancellationToken>()));
        }

        private static DefaultHttpContext SetupDefaultContextWithResponseBodyStream()
        {
            Stream bodyStream = new MemoryStream();
            var defaultContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "testuser")
                    }, "IsActivityHost")), 
                //Session = new CacheProfile() { Duration = 5 }
            };

            var response = new HttpResponseFeature
            {
                Body = bodyStream,
            };

            var featureCollection = new FeatureCollection();
            featureCollection.Set<IHttpResponseFeature>(response);
            defaultContext.Initialize(featureCollection);

            return defaultContext;
        }

        private static List<ActivityDto> GetSampleListActivityDto()
        {
            return new List<ActivityDto>()
            {
                new ActivityDto()
                {
                    Id = Guid.Parse("27906324-5442-4afc-9b9e-bf1a831e5b14"),
                    isPrivate = false,
                    Title = "Future Activity 5",
                    Date = DateTime.Parse("2023-10-03T09:18:31.329983Z"),
                    Description = "Activity 5 months in future",
                    Category = "drinks",
                    City = "London",
                    Venue = "Punch and Judy",
                    HostUsername = "bob",
                    IsCancelled = false,
                    Attendees = new List<AttendeeDto>()
                    {
                        new AttendeeDto
                        {
                            Username = "jane",
                            DisplayName = "Jane",
                            Bio = null,
                            Image = null,
                            Following = false,
                            FollowersCount = 0,
                            FollowingCount = 1
                        },
                        new AttendeeDto
                        {
                            Username = "jane",
                            DisplayName = "Jane",
                            Bio = null,
                            Image = null,
                            Following = false,
                            FollowersCount = 0,
                            FollowingCount = 1
                        }
                    }
                }
            };
        }

        private static Activity GetSampleActivity()
        {
            return new Activity
            {
                Id = Guid.Parse("27906324-5442-4afc-9b9e-bf1a831e5b14"),
                Title = "Future Activity 5",
                Date = DateTime.Parse("2023-10-03T09:18:31.329983Z"),
                Description = "Activity 5 months in future",
                Category = "drinks",
                City = "London",
                Venue = "Punch and Judy",
                IsCancelled = false,
                Comments = new List<Comment>(),
                Attendees = new List<ActivityAttendee>()
                {
                    new ActivityAttendee
                    {
                        AppUser = new AppUser { UserName = "Jane" },
                        IsHost = true
                    },
                    new ActivityAttendee
                    {
                        AppUser = new AppUser { UserName = "Bob" }
                    }
                }
            };
        }
    }
}