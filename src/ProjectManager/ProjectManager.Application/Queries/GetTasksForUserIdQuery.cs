using MediatR;
using ProjectManager.Application.Common;
using ProjectManager.Domain.Contracts;

namespace ProjectManager.Application.Queries
{
    public record GetTasksForUserIdQuery(int Id) : IRequest<CommandResult>;

    public class GetTasksForUserIdQueryHandler : IRequestHandler<GetTasksForUserIdQuery, CommandResult>
    {
        private readonly IProjectTaskRepository _taskRepo;

        public GetTasksForUserIdQueryHandler(IProjectTaskRepository taskRepo)
        {
            _taskRepo = taskRepo;
        }

        public async Task<CommandResult> Handle(GetTasksForUserIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var tasks = await _taskRepo.GetTasksForUser(request.Id);

                if (tasks is null)
                    return CommandResult.Failed("User not found", 404);

                return CommandResult.Success(tasks);
            }
            catch (Exception e)
            {
                return CommandResult.InternalServerError(e);
            }
        }
    }
}
