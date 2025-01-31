using MediatR;
using ProjectManager.Application.Common;
using ProjectManager.Domain.Contracts;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Commands
{
    public record AddTaskCommand(ProjectTask Task) : IRequest<CommandResult>;

    public class AddTaskCommandHandler : IRequestHandler<AddTaskCommand, CommandResult>
    {
        private readonly IProjectTaskRepository _taskRepo;

        public AddTaskCommandHandler(IProjectTaskRepository taskRepo)
        {
            _taskRepo = taskRepo;
        }

        public async Task<CommandResult> Handle(AddTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _taskRepo.AddTask(request.Task.Id, request.Task);
                return CommandResult.Success(request.Task);
            }
            catch (Exception e)
            {
                return CommandResult.InternalServerError(e);
            }
        }
    }
}
