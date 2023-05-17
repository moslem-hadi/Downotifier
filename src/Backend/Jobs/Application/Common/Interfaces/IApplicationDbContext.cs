using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<ApiCallJob> ApiCallJobs { get; }
    //DbSet<Notification> Notifications{ get; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
