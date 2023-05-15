﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<ApiCallJob> ApiCallJobs { get; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
