using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoList.OutboxProcess.Repository;

namespace ToDoList.OutboxProcess;

public static class OutboxFunction
{
    [FunctionName("TriggerFunction")]
    public static async Task Run([TimerTrigger("0 * * * * *", RunOnStartup = true)]TimerInfo myTimer, ILogger log)
    {
        log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

        //var stringConnection = "Server=localhost;Port=5432;User ID=postgres;Password=admin;Database=postgres";
        var stringConnection = Environment.GetEnvironmentVariable("DbConnectionString");

        var repository = new OutboxRepository(stringConnection);

        var outboxes = await repository.GetOutboxes();

        var ids = new List<Guid>();

        foreach (var outbox in outboxes)
        {
            log.LogInformation("The event: {event} with payload: {object} was processed.", outbox.Type, outbox.Event);

            ids.Add(outbox.Id);
        }
                
        if (ids.Count > 0)
            await repository.SetPublished(ids);

        await repository.DisposeAsync();

        log.LogInformation($"Finished function executed at: {DateTime.Now}");
    }
}