using System.Linq.Expressions;
using ToDoList.Api.Model;
using ToDoList.Api.Repository;

namespace ToDoList.Api.UseCase;

public class ActivityUseCase : IActivityUseCase
{
    private readonly IActivityRepository _activityRepository;

    public ActivityUseCase(IActivityRepository activityRepository)
    {
        _activityRepository = activityRepository;
    }

    public const int _maxPageSize = 50;
    public const int _minPage = 1;



    public async Task<Activity?> GetById(Guid Id, CancellationToken cancellationToken = default)
    {
        var result = await _activityRepository.GetById(Id, cancellationToken);

        return result;
    }

    public async Task<bool> Add(Activity activity, CancellationToken cancellationToken = default)
    {
        return await _activityRepository.Add(activity, cancellationToken);
    }

    public async Task<bool?> NextStatus(Guid Id, CancellationToken cancellationToken = default)
    {
        var activity = await _activityRepository.GetById(Id);

        if (activity == null)
            return null;

        var notAcceptedStatusToMove = new[] { Status.Finished, Status.Canceled };

        if (notAcceptedStatusToMove.Contains(activity.Status))
            return false;

        activity.NextStatus();

        await _activityRepository.Update(activity, cancellationToken);

        return true;
    }

    public async Task UpdateRange(IEnumerable<Activity> activities, CancellationToken cancellationToken = default)
    {
        await _activityRepository.UpdateRange(activities, cancellationToken);
    }

    public async Task<IEnumerable<Activity>> Get(Expression<Func<Activity, bool>>? predicate = default, int page = _minPage, int pageSize = _maxPageSize, CancellationToken cancellationToken = default)
    {
        if(pageSize > _maxPageSize)
            pageSize = _maxPageSize;

        if(page < _minPage)
            page = _minPage;

        var result = await _activityRepository.Get(predicate, page, pageSize, cancellationToken);

        if(!result.Any())
            return Enumerable.Empty<Activity>();

        return result;
    }

    public async Task<int> Count(Expression<Func<Activity, bool>>? predicate = default, CancellationToken cancellationToken = default)
    {
        return await _activityRepository.Count(predicate, cancellationToken);
    }

    public async Task<bool?> Cancel(Guid Id, CancellationToken cancellationToken = default)
    {
        var activity = await _activityRepository.GetById(Id);

        if (activity == null)
            return null;

        var notAcceptedStatusToCancel = new[] { Status.Finished, Status.Canceled };

        if (notAcceptedStatusToCancel.Contains(activity.Status))
            return false;

        activity.CancelStatus();

        await _activityRepository.Update(activity, cancellationToken);

        return true;
    }
}
