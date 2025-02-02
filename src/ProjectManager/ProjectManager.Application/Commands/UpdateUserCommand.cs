using MediatR;
using ProjectManager.Application.Common;
using ProjectManager.Application.Models;
using ProjectManager.Domain.Contracts;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Commands
{
    public record UpdateUserCommand(User User) : IRequest<CommandResult>;

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, CommandResult>
    {
        private readonly IUserRepository _userRepo;

        public UpdateUserCommandHandler(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }
        
        public async Task<CommandResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepo.GetUserById(request.User.Id);

                if (user is null)
                    return CommandResult.Failed("User not found", 404);

                user.Name = request.User.Name ?? user.Name;
                user.Email = request.User.Email ?? user.Email;
                user.Role = request.User.Role ?? user.Role;

                await _userRepo.Commit();
                
                return CommandResult.Success(UserDto.FromUser(user));
            }
            catch (Exception e)
            {
                return CommandResult.InternalServerError(e);
            }
        }
    }
}
