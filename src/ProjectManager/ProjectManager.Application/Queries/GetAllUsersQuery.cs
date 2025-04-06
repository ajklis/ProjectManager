using MediatR;
using Microsoft.Extensions.Logging;
using ProjectManager.Application.Common;
using ProjectManager.Application.Extensions;
using ProjectManager.Domain.Contracts;

namespace ProjectManager.Application.Queries
{
    public record GetAllUsersQuery() : IRequest<CommandResult>;

    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, CommandResult>
    {
        private readonly ILogger<GetAllUsersQueryHandler> _logger;
        private readonly IUserRepository _userRepo;

        public GetAllUsersQueryHandler(ILogger<GetAllUsersQueryHandler> logger, IUserRepository userRepo)
        {
            _logger = logger;
            _userRepo = userRepo;
        }

        public async Task<CommandResult> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var users = await _userRepo.GetAll();
                return CommandResult.Success(users);//users?.ToUserDtoList());
            }
            catch (Exception e)
            {
                return CommandResult.InternalServerError(e);
            }
        }
    }
}
