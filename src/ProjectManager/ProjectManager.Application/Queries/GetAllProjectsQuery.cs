using MediatR;
using ProjectManager.Application.Common;
using ProjectManager.Domain.Contracts;

namespace ProjectManager.Application.Queries
{
    public record GetAllProjectsQuery() : IRequest<CommandResult>;

    public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, CommandResult>
    {
        private readonly IProjectRepository _projectRepo;

        public GetAllProjectsQueryHandler(IProjectRepository projectRepo)
        {
            _projectRepo = projectRepo;
        }

        public async Task<CommandResult> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var projects = await _projectRepo.GetAll();
                return CommandResult.Success(projects);
            }
            catch (Exception e)
            {
                return CommandResult.InternalServerError(e);
            }
        }
    }
}
