using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.API.Models;
using ProjectManager.Application.Commands;
using ProjectManager.Application.Queries;
using ProjectManager.Domain.Entities;

namespace ProjectManager.API.Controllers
{
    public class ProjectsController : BaseController
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/projects/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
            => FromCommandResult(await _mediator.Send(new GetAllProjectsQuery()));

        // GET: api/projects/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
            => FromCommandResult(await _mediator.Send(new GetProjectByIdQuery(id)));

        // POST: api/projects
        [HttpPost()]
        public async Task<IActionResult> Details([FromBody] IdPostModel model)
            => FromCommandResult(await _mediator.Send(new GetProjectByIdQuery(model.Id)));

        // POST: api/projects/add
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] Project project)
            => FromCommandResult(await _mediator.Send(new AddProjectCommand(project)));

        // POST: api/projects/update
        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] Project project)
            => FromCommandResult(await _mediator.Send(new UpdateProjectCommand(project.Id, project.Name, project.Description, project.Status, project.Priority, project.StartDate, project.EndDate)));

        // GET: api/projects/delete/id
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
            => FromCommandResult(await _mediator.Send(new DeleteProjectCommand(id)));
    }
}
