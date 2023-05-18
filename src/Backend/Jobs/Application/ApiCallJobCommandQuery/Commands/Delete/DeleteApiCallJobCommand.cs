using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Shared.Messaging;
using static Shared.Constants;

namespace Application.ApiCallJobCommandQuery.Commands.Delete;

public record DeleteApiCallJobCommand(int Id) : IRequest;

public class DeleteShipCommandHandler : IRequestHandler<DeleteApiCallJobCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMessagePublisher _messagePublisher;

    public DeleteShipCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMessagePublisher messagePublisher)
    {
        _context = context;
        _currentUserService = currentUserService;
        _messagePublisher = messagePublisher;
    }

    public async Task Handle(DeleteApiCallJobCommand request, CancellationToken cancellationToken)
    {

        var entity = await _context.ApiCallJobs
          .FindAsync(new object[] { request.Id }, cancellationToken)
          ?? throw new NotFoundException(nameof(ApiCallJob), request.Id);
        if (entity.UserId == Guid.Parse(_currentUserService.UserId!))
        {
            entity.AddDomainEvent(new ApiCallJobDeletedEvent(entity));
            _context.ApiCallJobs.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await _messagePublisher.PublishAsync(QueueConstants.JobDeleteQueue, entity);

        }
    }
}
