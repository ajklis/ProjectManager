using MediatR;
using ProjectManager.Application.Common;
using ProjectManager.Application.Models;
using ProjectManager.Domain.Contracts;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Commands
{
    public record AddProjectCommand(Project Project) : IRequest<CommandResult>;

    public class AddProjectCommandHandler : IRequestHandler<AddProjectCommand, CommandResult>
    {
        private readonly IProjectRepository _projectRepo;

        public AddProjectCommandHandler(IProjectRepository projectRepo)
        {
            _projectRepo = projectRepo;
        }
        
        public async Task<CommandResult> Handle(AddProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _projectRepo.AddProject(request.Project);
                return CommandResult.Success(ProjectDto.FromProject(request.Project));
            }
            catch (Exception e)
            {
                return CommandResult.InternalServerError(e);
            }   
        }
    }
}
