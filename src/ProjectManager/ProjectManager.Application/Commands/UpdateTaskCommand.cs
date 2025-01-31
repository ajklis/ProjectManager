using MediatR;
using ProjectManager.Application.Common;
using ProjectManager.Domain.Contracts;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Commands
{
    public record UpdateTaskCommand(ProjectTask Task) : IRequest<CommandResult>;

    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, CommandResult>
    {
        private readonly IProjectTaskRepository _taskRepo;

        public UpdateTaskCommandHandler(IProjectTaskRepository taskRepo)
        {
            _taskRepo = taskRepo;
        }

        public async Task<CommandResult> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var task = await _taskRepo.Update(request.Task);

                if (task is null)
                    return CommandResult.Failed("Project task not found", 404);

                return CommandResult.Success(task);
            }
            catch (Exception e)
            {
                return CommandResult.InternalServerError(e);
            }
        }
    }
}
