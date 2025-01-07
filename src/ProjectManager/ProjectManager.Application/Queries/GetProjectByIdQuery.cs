using MediatR;
using ProjectManager.Application.Common;
using ProjectManager.Domain.Contracts;

namespace ProjectManager.Application.Queries
{
    public record GetProjectByIdQuery(int Id) : IRequest<CommandResult>;

    public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, CommandResult>
    {
        private readonly IProjectRepository _projectRepo;

        public GetProjectByIdQueryHandler(IProjectRepository projectRepo)
        {
            _projectRepo = projectRepo;
        }

        public async Task<CommandResult> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var project = await _projectRepo.GetProjectById(request.Id);

                if (project is null)
                    return CommandResult.Failed("Project not found", 404);

                return CommandResult.Success(project);
            }
            catch (Exception e)
            {
                return CommandResult.InternalServerError(e);
            }
        }
    }
}
