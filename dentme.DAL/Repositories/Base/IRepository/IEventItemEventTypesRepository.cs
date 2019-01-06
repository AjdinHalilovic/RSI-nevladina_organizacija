using Core.Entities.Base;
using System.Collections.Generic;

namespace DAL.Repositories.Base.IRepository
{
    public interface IEventItemEventTypesRepository:IRepository<EventItemEventType,int>
    {
        List<int> RemoveByEventItemId(int id);
        IEnumerable<EventItemEventType> GetByEventItemId(int Id);
        List<int> Remove(int eventItemId);
    }
}
