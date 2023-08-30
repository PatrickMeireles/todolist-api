using System.Linq.Expressions;
using ToDoList.Api.Model;
using ToDoList.Api.Repository;

namespace ToDoList.Api.UseCase;

public class OutboxUseCase : IOutboxUseCase
{
    private readonly IOutboxRepository _outboxRepository; 
    public const int _maxPageSize = 50;
    public const int _minPage = 1;

    public OutboxUseCase(IOutboxRepository outboxRepository)
    {
        _outboxRepository = outboxRepository;
    }

    public async Task UpdateRange(IEnumerable<Outbox> outboxes, CancellationToken cancellationToken = default)
    {
        await _outboxRepository.UpdateRange(outboxes, cancellationToken);
    }

    public async Task<IEnumerable<Outbox>> Get(Expression<Func<Outbox, bool>>? predicate = default, int page = _minPage, int pageSize = _maxPageSize, CancellationToken cancellationToken = default)
    {
        if (pageSize > _maxPageSize)
            pageSize = _maxPageSize;

        if (page < _minPage)
            page = _minPage;

        return await _outboxRepository.Get(predicate, page, pageSize, cancellationToken);
    }
}