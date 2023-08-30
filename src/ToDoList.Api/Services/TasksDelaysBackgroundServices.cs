using Quartz;
using System.Linq.Expressions;
using ToDoList.Api.Model;
using ToDoList.Api.UseCase;

namespace ToDoList.Api.Services;

public class TasksDelaysBackgroundServices : IJob
{
    private readonly IActivityUseCase _activityUseCase;
    private readonly ILogger<TasksDelaysBackgroundServices> _logger;
    private const int _size = 25;

    public TasksDelaysBackgroundServices(IActivityUseCase activityUseCase, ILogger<TasksDelaysBackgroundServices> logger)
    {
        _activityUseCase = activityUseCase;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var openActivies = new[] { Status.Created, Status.InProgress };

        Expression<Func<Activity, bool>> expression = c =>
            openActivies.Contains(c.Status) &&
            c.DateEstimatedFinish.HasValue &&
            c.DateEstimatedFinish.Value.Date < DateTime.Now.Date &&
        c.Delayed == false;

        var countTasksDelays = await _activityUseCase.Count(expression, context.CancellationToken);

        _logger.LogInformation("{count} late tasks were found.", countTasksDelays);

        var _page = 1;

        while (countTasksDelays > 0)
        {
            var activiesDelayed = await _activityUseCase.Get(expression, _page, _size, context.CancellationToken);

            var activies = new List<Activity>(activiesDelayed.Count());

            foreach (var activity in activiesDelayed)
            {
                _logger.LogInformation("The task {name} with Date Estimated Finish in {DateEstimatedFinish} is late.", activity.Name, activity.DateEstimatedFinish);

                activity.IsDelay();
                activies.Add(activity);
            }

            await _activityUseCase.UpdateRange(activies, context.CancellationToken);

            countTasksDelays = countTasksDelays - _size;
        }
    }
}
