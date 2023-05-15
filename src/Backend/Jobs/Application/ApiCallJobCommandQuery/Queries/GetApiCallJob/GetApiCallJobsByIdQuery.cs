using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

    public GetShipByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiCallJobDto?> Handle(GetApiCallJobsByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.ApiCallJobs.FirstOrDefaultAsync(a => a.Id == request.ShipId);
        return _mapper.Map<ApiCallJobDto>(entity);

    }
}
