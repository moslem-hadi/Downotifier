using Application.Common.Exceptions;
using Application.Common.Interfaces;

namespace Application.ApiCallJobCommandQuery.Commands.Update;

public class UpdateApiCallJobCommand : ApiCallJobDto, IRequest 
{
}

public class UpdateApiCallJobCommandHandler : IRequestHandler<UpdateApiCallJobCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public UpdateApiCallJobCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task Handle(UpdateApiCallJobCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ApiCallJobs
        .FindAsync(new object[] { request.Id }, cancellationToken)
        ?? throw new NotFoundException(nameof(ApiCallJob), request.Id);
        if (entity.UserId == Guid.Parse(_currentUserService.UserId!))
        {

            //TODO: fix
            entity.Title = request.Title;
            entity.Headers = request.Headers;
            entity.JsonBody = request.JsonBody;
            entity.Method = request.Method;
            entity.MonitoringInterval = request.MonitoringInterval;
            entity.Url = request.Url;
            entity.Notifications = request.Notifications;
            entity.UserId = request.UserId;
            // _context.Entry(entity).CurrentValues.SetValues((ApiCallJob)(request));
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
