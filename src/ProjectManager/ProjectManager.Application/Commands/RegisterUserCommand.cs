using MediatR;
using ProjectManager.Application.Common;
using ProjectManager.Application.Models;
using ProjectManager.Domain.Contracts;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Commands
{
    public record RegisterUserCommand (string Name, string Email, string Password): IRequest<CommandResult>;

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, CommandResult>
    {
        private readonly IUserRepository _userRepo;
        private readonly IPasswordHashingService _passwordHashingService;

        public RegisterUserCommandHandler(IUserRepository userRepo, IPasswordHashingService passwordHashingService)
        {
            _userRepo = userRepo;
            _passwordHashingService = passwordHashingService;
        }

        public async Task<CommandResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Name is null || request.Email is null)
                    return CommandResult.Failed("Incorrect user model", 400);

                if (await _userRepo.GetUserByEmail(request.Email) is not null)
                    return CommandResult.Failed("User already exists", 422);

                var user = new User
                {
                    Name = request.Name,
                    Email = request.Email,
                    HashedPassword = _passwordHashingService.GetHashedPassword(request.Password),
                    Role = Domain.Enums.UserRole.Unassigned,
                    CreatedAt = DateTime.Now
                };

                var users = await _userRepo.GetAll();
                if (users.Count() == 0)
                    user.Role = Domain.Enums.UserRole.Admin;

                await _userRepo.AddUser(user);

                return CommandResult.Success(UserDto.FromUser(user));
            }
            catch (Exception e)
            {
                return CommandResult.InternalServerError(e);
            }
        }
    }
}
