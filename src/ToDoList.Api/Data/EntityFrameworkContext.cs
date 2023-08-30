using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using ToDoList.Api.Events;
using ToDoList.Api.Model;

namespace ToDoList.Api.Data;

public class EntityFrameworkContext : DbContext
{
    private readonly ILogger<EntityFrameworkContext> _logger;

    public EntityFrameworkContext(DbContextOptions<EntityFrameworkContext> options, ILogger<EntityFrameworkContext> logger) : base(options)
    {
        _logger = logger;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<IDomainEvent>();

        modelBuilder.Entity<Activity>().ToTable("activies");
        modelBuilder.Entity<Outbox>().ToTable("outboxes");

        base.OnModelCreating(modelBuilder);
    }

    public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker.Entries<AggregateRoot>().Where(c => c.Entity.DomainEvents != null && c.Entity.DomainEvents.Any()).ToList();

        var events = domainEvents.SelectMany(c => c.Entity.DomainEvents);

        _logger.LogInformation("Were found {0} events to post save in outbox", events.Count());

        foreach(var @event in events)
        {
            var type = @event.GetType().Name;
            var outbox = Outbox.Create(@event, type);

            await Outboxes.AddAsync(outbox, cancellationToken);
        }

        domainEvents.ForEach(item => item.Entity.ClearEvents());

        var success = await SaveChangesAsync(cancellationToken) > 0;

        return true;
    }

    public DbSet<Activity> Activities { get; set; } = null!;
    public DbSet<Outbox> Outboxes { get; set; } = null!;
}

public static class MyModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
}