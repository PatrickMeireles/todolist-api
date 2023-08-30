using System.Linq.Expressions;
using ToDoList.Api.Model;

namespace ToDoList.Api.UseCase;

public interface IOutboxUseCase
{
    Task UpdateRange(IEnumerable<Outbox> outboxes, CancellationToken cancellationToken = default);

    Task<IEnumerable<Outbox>> Get(Expression<Func<Outbox, bool>>? predicate = default, int page = 1, int pageSize = 50, CancellationToken cancellationToken = default);
}
