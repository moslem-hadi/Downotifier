using Application.Common.Exceptions;
using Application.Common.Interfaces;

namespace Application.ApiCallJobCommandQuery.Commands.Delete;

public record DeleteApiCallJobCommand(int Id) : IRequest;

public class DeleteShipCommandHandler : IRequestHandler<DeleteApiCallJobCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteShipCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteApiCallJobCommand request, CancellationToken cancellationToken)
    {

        var entity = await _context.ApiCallJobs
          .FindAsync(new object[] { request.Id }, cancellationToken)
          ?? throw new NotFoundException(nameof(ApiCallJob), request.Id);

        entity.AddDomainEvent(new ApiCallJobDeletedEvent(entity));

        _context.ApiCallJobs.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
