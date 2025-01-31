using MediatR;
using ProjectManager.Application.Common;
using ProjectManager.Domain.Contracts;

namespace ProjectManager.Application.Queries
{
    public record GetTasksForProjectIdQuery(int id) : IRequest<CommandResult>;

    public class GetTasksForProjectByIdQueryHandler : IRequestHandler<GetTasksForProjectIdQuery, CommandResult>
    {
        private readonly IProjectTaskRepository _taskRepo;

        public GetTasksForProjectByIdQueryHandler(IProjectTaskRepository taskRepo)
        {
            _taskRepo = taskRepo;
        }

        public async Task<CommandResult> Handle(GetTasksForProjectIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var tasks = await _taskRepo.GetTasksForProject(request.id);

                if (tasks is null)
                    return CommandResult.Failed("Project not found", 404);

                return CommandResult.Success(tasks);
            }
            catch (Exception e)
            {
                return CommandResult.InternalServerError(e);
            }
        }
    }
}
