using Application.Common.Interfaces;

namespace Application.ApiCallJobCommandQuery.Commands.Create;

public partial class CreateApiCallJobCommand : ApiCallJobDto, IRequest<int> 
{ 
}

public class CreateApiCallJobCommandHandler : IRequestHandler<CreateApiCallJobCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public CreateApiCallJobCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<int> Handle(CreateApiCallJobCommand request, CancellationToken cancellationToken)
    { 
        var entity = _mapper.Map<ApiCallJob>(request);
        entity.UserId = Guid.Parse(_currentUserService.UserId);

        entity.AddDomainEvent(new ApiCallJobCreatedEvent(entity));
        _context.ApiCallJobs.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
