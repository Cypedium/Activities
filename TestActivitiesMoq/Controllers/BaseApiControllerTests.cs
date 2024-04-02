using API.Controllers;
using Application.Activities;
using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace TestActivitiesMoq.Controllers
{
    public class BaseApiControllerTests : ControllerBase
    {
        protected readonly Mock<IMediator> _mediatorMock;
        protected readonly BaseApiController _controller;

        public BaseApiControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(typeof(IMediator))).Returns(_mediatorMock.Object);

            var httpContextMock = new Mock<HttpContext>();
            var httpResponseMock = new Mock<HttpResponse>();
            httpContextMock.Setup(x => x.RequestServices).Returns(serviceProviderMock.Object);

            Stream bodyStream = new MemoryStream();

            _controller = new BaseApiController
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = SetupDefaultContextWithResponseBodyStream(bodyStream)
                }
            };
        }

        [Fact]
        public void HandleResult_ReturnsNotFound_WhenResultIsNull()
        {
            var result = _controller.HandleResult<ActivityDto>(null);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void HandleResult_ReturnsOk_WhenResultIsSuccessAndValueIsNotNull()
        {
            var items = GetSampleListActivityDto();

            var expectedResult = new Result<ActivityDto>()
            {
                IsSuccess = true,
                Value = items[0]
            };

            var result = _controller.HandleResult(expectedResult);

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(items[0], ((OkObjectResult)result).Value);
        }

        [Fact]
        public void HandlePageResult_ReturnsOk_WhenResultIsSuccessAndValueIsNotNull()
        {
            var items = GetSampleListActivityDto();
            var count = 1;
            var pageNumber = 1;
            var pageSize = 10;

            var expectedPagedResult = new PagedList<ActivityDto>(items, count, pageNumber, pageSize);

            var expectedResult = new Result<PagedList<ActivityDto>>()
            {
                IsSuccess = true,
                Value = expectedPagedResult
            };

            //Response null cant add pagination
            var result = _controller.HandlePagedResult(expectedResult);

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedPagedResult, ((OkObjectResult)result).Value);
        }

        private static DefaultHttpContext SetupDefaultContextWithResponseBodyStream(Stream bodyStream)
        {
            var defaultContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, "testuser")
                },
                "IsActivityHost"))
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
            var result = new List<ActivityDto>()
                    {
                        new ActivityDto()
                        {
                            Id = Guid.Parse("27906324-5442-4afc-9b9e-bf1a831e5b14"),
                            IsPrivate = false,
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
                                },
                            }
                        }
                    };

            return result;
        }
    }
}
