using ToDoList.Api.Model;
using ToDoList.Api.Repository.Base;

namespace ToDoList.Api.Repository;

public interface IOutboxRepository : IRepository<Outbox>
{
}
