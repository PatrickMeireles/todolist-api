using Quartz;
using System.Linq.Expressions;
using ToDoList.Api.Model;
using ToDoList.Api.UseCase;

namespace ToDoList.Api.Services;

public class OutboxProcessBackgroundServices : IJob
{
    private readonly IOutboxUseCase _outboxUseCase;
    private readonly ILogger<OutboxProcessBackgroundServices> _logger;

    public OutboxProcessBackgroundServices(IOutboxUseCase outboxUseCase, ILogger<OutboxProcessBackgroundServices> logger)
    {
        _outboxUseCase = outboxUseCase;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Started Outbox Process Job");

        Expression<Func<Outbox, bool>> expression = c => !c.Published && !c.PublishedAt.HasValue;

        var data = await _outboxUseCase.Get(expression, cancellationToken: context.CancellationToken);

        var count = data.Count();

        var itemsToUpdate = new List<Outbox>(count);

        _logger.LogInformation("There are {count} Outbox to process", count);

        foreach (var outbox in data)
        {
            outbox.SetPublish();

            itemsToUpdate.Add(outbox);

            _logger.LogInformation("The event: {event} with payload: {object} was processed.", outbox.Type, outbox.Event);
        }

        if (itemsToUpdate.Count > 0)
            await _outboxUseCase.UpdateRange(itemsToUpdate, cancellationToken: context.CancellationToken);


        _logger.LogInformation("Finished Outbox Process Job");
    }
}
