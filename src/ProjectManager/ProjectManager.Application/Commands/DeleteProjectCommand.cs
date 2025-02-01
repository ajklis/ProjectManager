using MediatR;
using ProjectManager.Application.Common;
using ProjectManager.Domain.Contracts;

namespace ProjectManager.Application.Commands
{
    public record DeleteProjectCommand(int Id) : IRequest<CommandResult>;

    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, CommandResult>
    {
        private readonly IProjectRepository _projectRepo;

        public DeleteProjectCommandHandler(IProjectRepository projectRepo)
        {
            _projectRepo = projectRepo;
        }

        public async Task<CommandResult> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var success = await _projectRepo.RemoveProjectById(request.Id);

                return success 
                    ? CommandResult.Success("Project deleted")
                    : CommandResult.Failed("Project not found", 404);
            }
            catch (Exception e)
            {
                return CommandResult.InternalServerError(e);
            }
        }
    }
}
