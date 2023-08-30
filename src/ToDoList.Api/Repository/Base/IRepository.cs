using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ToDoList.Api.Model;

namespace ToDoList.Api.Repository.Base;

public interface IRepository<T> where T : Entity
{
    Task<T?> GetById(Guid id, CancellationToken cancellationToken = default);

    Task<bool> Add(T entity, CancellationToken cancellationToken = default);

    Task Update(T entity, CancellationToken cancellationToken = default);

    Task UpdateRange(IEnumerable<T> activities, CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> Get(Expression<Func<T, bool>>? predicate = default, int page = 1, int pageSize = 50, CancellationToken cancellationToken = default);

    Task<int> Count(Expression<Func<T, bool>>? predicate = default, CancellationToken cancellationToken = default);
}
