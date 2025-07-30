using CleanArchitecture.Application.Abstractions.Clock;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Infrastructure.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CleanArchitecture.Infrastructure;

public sealed class ApplicationDbContext : DbContext, IUnitOfWork
{
    // private readonly IPublisher _publisher;
    //para que me agre el tipo de la clase de dominio en el json, es necesario usar TypeNameHandling.Auto
    private static readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.All, // Automatically include type information in serialized JSON
        NullValueHandling = NullValueHandling.Ignore,
        Formatting = Formatting.Indented
    };

    private readonly IDateTimeProvider _dateTimeProvider;
    public ApplicationDbContext(DbContextOptions options,IDateTimeProvider dateTimeProvider) : base(options)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            PublishDomainEventsToOutboxMessages();
            var result = await base.SaveChangesAsync(cancellationToken);

            // await PublishDomainEventsAsync();

            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("La excepcion por concurrencia se disparo", ex);
        }
    }
    /*
    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker
            .Entries<IEntity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity => 
            {
                var domainEvents = entity.GetDomainEvents();
                entity.ClearDomainEvents();
                return domainEvents;
            }).ToList();
        
        foreach(var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent);
        }

    }*/

    private void PublishDomainEventsToOutboxMessages()
    {
        var outboxMessages = ChangeTracker //changeTracker is used to track changes in the context
            .Entries<IEntity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.GetDomainEvents();
                entity.ClearDomainEvents();
                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage(
                Guid.NewGuid(),
                _dateTimeProvider.currentTime,
                domainEvent.GetType().Name, //get the name of the domain event type
               JsonConvert.SerializeObject(domainEvent, jsonSerializerSettings)
                ))
            .ToList();
        AddRange(outboxMessages); //con esto agragamos alos objetos a la memoria del entity framework para guardarlos en la base de datos   

    }


}