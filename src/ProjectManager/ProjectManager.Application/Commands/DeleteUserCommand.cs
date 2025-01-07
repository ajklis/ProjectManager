using MediatR;
using ProjectManager.Application.Common;
using ProjectManager.Domain.Contracts;

namespace ProjectManager.Application.Commands
{
    public record DeleteUserCommand(int Id) : IRequest<CommandResult>;

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, CommandResult>
    {
        private readonly IUserRepository _userRepo;

        public DeleteUserCommandHandler(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<CommandResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var res = await _userRepo.RemoveUserById(request.Id);
                return res
                    ? CommandResult.Success("User deleted")
                    : CommandResult.Failed("User not found", 404);
            }
            catch (Exception e)
            {
                return CommandResult.InternalServerError(e);
            }
        }
    }

}
