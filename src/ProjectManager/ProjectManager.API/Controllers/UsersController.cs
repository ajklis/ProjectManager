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
        public UsersController(IMediator mediator) : base(mediator) { }

        // GET: api/users/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
            => await HandleRequest(
                new GetAllProjectsQuery(), // query to be executed by mediator
                user => user.Role == Domain.Enums.UserRole.Admin); // predicate to not allow unauthorized users

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
            => await HandleRequest(
                new GetUserByIdQuery(id),
                user => user.Role == Domain.Enums.UserRole.Admin || user.Id == id);

        // POST: api/users
        [HttpPost()]
        public async Task<IActionResult> Details([FromBody] IdPostModel model)
            => await HandleRequest(
                new GetUserByIdQuery(model.Id),
                user => user.Role == Domain.Enums.UserRole.Admin || user.Id == model.Id);

        // POST: api/users/add
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] UserAddModel user)
            => await HandleRequest(
                new AddUserCommand(user.Name, user.Email, user.Password, user.Role));
                //user => user.Role == Domain.Enums.UserRole.Admin);

        // POST: api/users/update
        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] User update)
            => await HandleRequest(
                new UpdateUserCommand(update), 
                user => user.Role == Domain.Enums.UserRole.Admin || user.Id == update.Id);

        // GET: api/users/delete/5
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
            => await HandleRequest(
                new DeleteUserCommand(id),
                user => user.Role == Domain.Enums.UserRole.Admin || user.Id == id);

        // POST: api/users/delete
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] IdPostModel model)
            => await HandleRequest(
                new DeleteUserCommand(model.Id),
                user => user.Role == Domain.Enums.UserRole.Admin || user.Id == model.Id);
    }
}
