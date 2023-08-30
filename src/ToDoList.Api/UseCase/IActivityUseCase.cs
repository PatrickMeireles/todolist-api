using System.Linq.Expressions;
using ToDoList.Api.Model;

namespace ToDoList.Api.UseCase;

public interface IActivityUseCase
{
    Task<Activity?> GetById(Guid Id, CancellationToken cancellationToken = default);
    Task<bool> Add(Activity activity, CancellationToken cancellationToken = default);
    Task<bool?> NextStatus(Guid Id, CancellationToken cancellationToken = default);
    Task<bool?> Cancel(Guid Id, CancellationToken cancellationToken = default);
    Task UpdateRange(IEnumerable<Activity> activities, CancellationToken cancellationToken = default);
    Task<IEnumerable<Activity>> Get(Expression<Func<Activity, bool>>? predicate = default, int page = 1, int pageSize = 50, CancellationToken cancellationToken = default);
    Task<int> Count(Expression<Func<Activity, bool>>? predicate = default, CancellationToken cancellationToken = default);
}
