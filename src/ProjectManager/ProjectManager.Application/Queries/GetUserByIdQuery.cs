using MediatR;
using ProjectManager.Application.Common;
using ProjectManager.Application.Models;
using ProjectManager.Domain.Contracts;

namespace ProjectManager.Application.Queries
{
    public record GetUserByIdQuery(int Id) : IRequest<CommandResult>;

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, CommandResult>
    {
        private readonly IUserRepository _userRepo;

        public GetUserByIdQueryHandler(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<CommandResult> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepo.GetUserById(request.Id);

                if (user is null)
                    return CommandResult.Failed("User not found", 404);

                return CommandResult.Success(UserDto.FromUser(user));
            }
            catch(Exception e)
            {
                return CommandResult.InternalServerError(e);
            }
        }
    }
}
