using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Application.Common.Security;
using AutoMapper;
using Domain.Events;

namespace Application.ApiCallJobCommandQuery.Commands.Create;

[Authorize]
public class CreateApiCallJobCommand : ApiCallJobDto, IRequest<int>
{
    //public static implicit operator ApiCallJob(CreateApiCallJobCommand apiCallJob) => new()
    //{
    //    Title = apiCallJob.Title,
    //    Headers = apiCallJob.Headers,
    //    JsonBody = apiCallJob.JsonBody,
    //    Method = apiCallJob.Method,
    //    MonitoringInterval = apiCallJob.MonitoringInterval,
    //    Url = apiCallJob.Url,
    //    UserId = apiCallJob.UserId,
    //    Notifications = apiCallJob.Notifications
    //};
}

public class CreateApiCallJobCommandHandler : IRequestHandler<CreateApiCallJobCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateApiCallJobCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateApiCallJobCommand request, CancellationToken cancellationToken)
    {
        var entity = (ApiCallJob)request;
        //var entity = _mapper.Map<ApiCallJob>(request)

        entity.AddDomainEvent(new ApiCallJobCreatedEvent(entity));
        _context.ApiCallJobs.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
