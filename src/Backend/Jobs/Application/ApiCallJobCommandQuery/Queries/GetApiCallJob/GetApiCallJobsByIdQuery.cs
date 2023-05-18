using Application.Common.Interfaces;
using Application.Common.Security;

namespace Application.ApiCallJobCommandQuery.Queries.GetApiCallJob;


[Authorize]
public record GetApiCallJobsByIdQuery(int ShipId) : IRequest<ApiCallJobDto?>
{
}

public class GetShipByIdQueryHandler : IRequestHandler<GetApiCallJobsByIdQuery, ApiCallJobDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetShipByIdQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<ApiCallJobDto?> Handle(GetApiCallJobsByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.ApiCallJobs
            .FirstOrDefaultAsync(a => a.UserId== Guid.Parse(_currentUserService.UserId!) && a.Id == request.ShipId);
        return _mapper.Map<ApiCallJobDto>(entity);

    }
}
