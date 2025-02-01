using MediatR;
using ProjectManager.Application.Common;
using ProjectManager.Domain.Contracts;

namespace ProjectManager.Application.Commands
{
    public record DeleteTaskCommand(int Id) : IRequest<CommandResult>;

    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, CommandResult>
    {
        private readonly IProjectTaskRepository _taskRepo;

        public DeleteTaskCommandHandler(IProjectTaskRepository taskRepo)
        {
            _taskRepo = taskRepo;
        }

        public async Task<CommandResult> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var success = await _taskRepo.RemoveTaskById(request.Id);

                return success 
                    ? CommandResult.Success("Project task deleted")
                    : CommandResult.Failed("Project task not found", 404);
            }
            catch (Exception e)
            {
                return CommandResult.InternalServerError(e);
            }
        }
    }
}
