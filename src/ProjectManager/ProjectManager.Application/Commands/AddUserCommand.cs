using MediatR;
using ProjectManager.Application.Common;
using ProjectManager.Application.Models;
using ProjectManager.Domain.Contracts;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Commands
{
    public record AddUserCommand(User User) : IRequest<CommandResult>;

    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, CommandResult>
    {
        private readonly IUserRepository _userRepo;
        
        public AddUserCommandHandler(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }
        
        public async Task<CommandResult> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.User is null || request.User.Name is null || request.User.Email is null)
                    return CommandResult.Failed("Incorrect user model", 400);

                await _userRepo.AddUser(request.User);
                
                return CommandResult.Success(UserDto.FromUser(request.User));
            }
            catch (Exception e)
            {
                return CommandResult.InternalServerError(e);
            }
        }
    }
}
