using Application.Common.Interfaces;
using Application.Common.Security;

namespace Application.ApiCallJobCommandQuery.Queries.GetApiCallJob;


[Authorize]
public record GetApiCallJobsByIdQuery(int apiCallJobId) : IRequest<ApiCallJobDto?>
{
}

public class GetApiCallJobsByIdQueryHandler : IRequestHandler<GetApiCallJobsByIdQuery, ApiCallJobDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetApiCallJobsByIdQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<ApiCallJobDto?> Handle(GetApiCallJobsByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.ApiCallJobs.Include(a=> a.Notifications)
            .FirstOrDefaultAsync(a => a.UserId== Guid.Parse(_currentUserService.UserId!) && a.Id == request.apiCallJobId);
        return _mapper.Map<ApiCallJobDto>(entity);

    }
}
