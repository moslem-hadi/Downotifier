﻿using Microsoft.EntityFrameworkCore;
using Domain.Common.Contracts;

namespace MediatR;

public static class MediatorExtensions
{
    public static async Task DispatchDomainEvents(this IMediator mediator, DbContext context, CancellationToken cancellationToken) 
    {
        var entities = context.ChangeTracker
            .Entries<IBaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity);

        var domainEvents = entities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        entities.ToList().ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent);
    }
}
