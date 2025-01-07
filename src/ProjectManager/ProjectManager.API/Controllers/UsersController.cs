using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.API.Models;
using ProjectManager.Application.Commands;
using ProjectManager.Application.Queries;
using ProjectManager.Domain.Entities;

namespace ProjectManager.API.Controllers
{
    public class UsersController : BaseController
    {
        private IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/users/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAll() 
            => FromCommandResult(await _mediator.Send(new GetAllUsersQuery()));

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id) 
            => FromCommandResult(await _mediator.Send(new GetUserByIdQuery(id)));

        // POST: api/users
        [HttpPost()]
        public async Task<IActionResult> Details([FromBody] IdPostModel model)
            => FromCommandResult(await _mediator.Send(new GetUserByIdQuery(model.Id)));

        // POST: api/users/add
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] User user) 
            => FromCommandResult(await _mediator.Send(new AddUserCommand(user)));

        // POST: api/users/update
        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] User update) 
            => FromCommandResult(await _mediator.Send(new UpdateUserCommand(update)));

        // GET: api/users/delete/5
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
            => FromCommandResult(await _mediator.Send(new DeleteUserCommand(id)));

        // POST: api/users/delete
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] IdPostModel model)
            => FromCommandResult(await _mediator.Send(new DeleteUserCommand(model.Id)));
    }
}
