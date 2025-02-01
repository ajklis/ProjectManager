using MediatR;
using ProjectManager.Application.Common;
using ProjectManager.Application.Models;
using ProjectManager.Domain.Contracts;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Enums;

namespace ProjectManager.Application.Commands
{
    public class UpdateProjectCommand : Project, IRequest<CommandResult>
    {
        public UpdateProjectCommand(int id, string name, string? description, ProjectTaskStatus status, DateTime? startDate, DateTime? endDate)
            : base (id, name, description, status, startDate, endDate) {  }
    }

    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, CommandResult>
    {
        private readonly IProjectRepository _projectRepo;

        public UpdateProjectCommandHandler(IProjectRepository projectRepo)
        {
            _projectRepo = projectRepo;
        }

        public async Task<CommandResult> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var project = await _projectRepo.GetProjectById(request.Id);

                if (project is null)
                    return CommandResult.Failed("Project not found", 404);

                project.Description = request.Description;
                project.Name = request.Name ?? project.Name;
                project.StartDate = project.StartDate.Equals(request.StartDate) ? project.StartDate : request.StartDate;
                project.EndDate = request.EndDate ?? project.EndDate;
                project.Status = project.Status.Equals(request.Status) ? project.Status : request.Status;

                return CommandResult.Success(ProjectDto.FromProject(project));
            }
            catch (Exception e)
            {
                return CommandResult.InternalServerError(e);
            }
        }
    }
}
