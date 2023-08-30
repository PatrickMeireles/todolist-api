using ToDoList.Api.Data;
using ToDoList.Api.Model;
using ToDoList.Api.Repository.Base;

namespace ToDoList.Api.Repository;

public class OutboxRepository : Repository<Outbox>, IOutboxRepository
{
    public OutboxRepository(EntityFrameworkContext context) : base(context)
    {
    }
}
