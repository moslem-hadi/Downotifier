using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Application.Common.Security;
using AutoMapper;
using Domain.Events;
using Application.Common.Mappings;
using AutoMapper.Internal;

namespace Application.ApiCallJobCommandQuery.Commands.Create;

[Authorize]
public partial class CreateApiCallJobCommand : ApiCallJobDto, IRequest<int> 
{ 
}

public class CreateApiCallJobCommandHandler : IRequestHandler<CreateApiCallJobCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateApiCallJobCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateApiCallJobCommand request, CancellationToken cancellationToken)
    { 
        var entity = _mapper.Map<ApiCallJob>(request); 

        entity.AddDomainEvent(new ApiCallJobCreatedEvent(entity));
        _context.ApiCallJobs.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
