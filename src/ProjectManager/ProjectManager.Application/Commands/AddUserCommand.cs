using MediatR;
using ProjectManager.Application.Common;
using ProjectManager.Application.Models;
using ProjectManager.Domain.Contracts;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Enums;

namespace ProjectManager.Application.Commands
{
    public record AddUserCommand(string Name, string Email, string Password, UserRole Role) : IRequest<CommandResult>;

    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, CommandResult>
    {
        private readonly IUserRepository _userRepo;
        private readonly IPasswordHashingService _passwordHashingService;
        
        public AddUserCommandHandler(IUserRepository userRepo, IPasswordHashingService passwordHashingService)
        {
            _userRepo = userRepo;
            _passwordHashingService = passwordHashingService;
        }
        
        public async Task<CommandResult> Handle(AddUserCommand request, CancellationToken cancellationToken)
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
                    Role = request.Role,
                    CreatedAt = DateTime.Now
                };

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
