using ToDoList.Api.Data;
using ToDoList.Api.Model;
using ToDoList.Api.Repository.Base;

namespace ToDoList.Api.Repository;

public class ActivityRepository : Repository<Activity>, IActivityRepository
{
    public ActivityRepository(EntityFrameworkContext context) : base(context)
    {
    }
}
