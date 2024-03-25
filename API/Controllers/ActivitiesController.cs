using Domain;
using Microsoft.AspNetCore.Mvc;
using Application.Activities;
using Microsoft.AspNetCore.Authorization;
using MediatR;

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController {

        private readonly IMediator _mediator;

        public ActivitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet] //api/activities
        public async Task<IActionResult> GetActivities([FromQuery]ActivityParams param)
        {
            return HandlePagedResult(await _mediator.Send(new List.Query { Params = param }));
        }

        [HttpGet("{id}")] //api/activities/id
        public async Task<IActionResult> GetActivity(Guid id)
        {
            return HandleResult(await _mediator.Send(new Details.Query { Id = id }));
        }

        [HttpPost]
        public async Task<IActionResult> CreateActivity(Activity activity)
        {
            return HandleResult(await _mediator.Send(new Create.Command { Activity = activity }));
        }

        [Authorize(Policy = "IsActivityHost")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditActivity(Guid id, Activity activity)
        {
            activity.Id = id;
            return HandleResult(await _mediator.Send(new Edit.Command { Activity = activity, }));
        }

        [Authorize(Policy = "IsActivityHost")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id)
        {
            return HandleResult(await _mediator.Send(new Delete.Command { Id = id }));
        }

        [HttpPost("{id}/attend")]
        public async Task<IActionResult> Attend(Guid id)
        {
            return HandleResult(await _mediator.Send(new UpdateAttendence.Command { Id = id }));
        }
    }
}