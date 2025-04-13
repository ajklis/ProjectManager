using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.API.Models;
using ProjectManager.Application.Commands;

namespace ProjectManager.API.Controllers
{
    public class RegisterController : BaseController
    {
        public RegisterController(IMediator mediator) : base(mediator) { }

        [HttpPost()]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterModel model)
        {
            return FromCommandResult(await _mediator.Send(new RegisterUserCommand(model.Name, model.Email, model.Password)));
        }
    }
}
