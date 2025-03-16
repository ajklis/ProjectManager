using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.Commands;
using ProjectManager.Application.Queries;
using ProjectManager.Domain.Entities;

namespace ProjectManager.API.Controllers
{
    public class TasksController : BaseController
    {
        public TasksController(IMediator mediator) : base(mediator) { }

        // POST: api/tasks/add
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] ProjectTask task)
            => FromCommandResult(await _mediator.Send(new AddTaskCommand(task)));

        // GET: api/tasks/detele/5
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
            => FromCommandResult(await _mediator.Send(new DeleteTaskCommand(id)));

        // POST: api/tasks/update
        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] ProjectTask task)
            => FromCommandResult(await _mediator.Send(new UpdateTaskCommand(task)));

        // GET: api/tasks/project/5
        [HttpGet("project/{id}")]
        public async Task<IActionResult> GetTasksForProjectId(int id)
            => FromCommandResult(await _mediator.Send(new GetTasksForProjectIdQuery(id)));

        // GET: api/tasks/user/5
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetTasksForUserId(int id)
            => FromCommandResult(await _mediator.Send(new GetTasksForUserIdQuery(id)));
    }
}
