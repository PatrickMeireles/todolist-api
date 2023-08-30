using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ToDoList.Api.Data;
using ToDoList.Api.Model;

namespace ToDoList.Api.Repository.Base;

public abstract class Repository<T> : IRepository<T> where T : Entity
{
    private readonly EntityFrameworkContext _context;
    private readonly DbSet<T> _dbSet;

    protected Repository(EntityFrameworkContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public virtual async Task<T?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _dbSet
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        return result;
    }

    public virtual async Task<bool> Add(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);

        return await _context.CommitAsync(cancellationToken);
    }

    public virtual async Task Update(T entity, CancellationToken cancellationToken = default)
    {
        
        _context.Entry(entity).State = EntityState.Modified;
        _dbSet.Update(entity);

        await _context.CommitAsync(cancellationToken);
    }

    public virtual async Task UpdateRange(IEnumerable<T> activities, CancellationToken cancellationToken = default)
    {
        _dbSet.UpdateRange(activities);

        await _context.CommitAsync(cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> Get(Expression<Func<T, bool>>? predicate = default, int page = 1, int pageSize = 50, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsNoTracking();

        if (predicate is not null)
            query = query.Where(predicate);

        query = query.Skip(Math.Abs((page - 1) * pageSize)).Take(pageSize);

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<int> Count(Expression<Func<T, bool>>? predicate = default, CancellationToken cancellationToken = default)
    {
        if(predicate is null)
            return await _dbSet.CountAsync(cancellationToken);

        return await _dbSet.CountAsync(predicate, cancellationToken);
    }
}
