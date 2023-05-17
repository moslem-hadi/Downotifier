using AutoMapper;
using AutoMapper.QueryableExtensions;
using Application.Common.Interfaces;
using MediatR;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Common.Security;

namespace Application.ApiCallJobCommandQuery.Queries.GetPaged;

[Authorize]
public class GetApiCallJobsQuery : IRequest<PaginatedList<ApiCallJobDto>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetShipsQueryHandler : IRequestHandler<GetApiCallJobsQuery, PaginatedList<ApiCallJobDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetShipsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ApiCallJobDto>> Handle(GetApiCallJobsQuery request, CancellationToken cancellationToken)
    {
        return await _context.ApiCallJobs.OrderByDescending(a=> a.Id)
            .ProjectTo<ApiCallJobDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.PageSize);
    }
}
