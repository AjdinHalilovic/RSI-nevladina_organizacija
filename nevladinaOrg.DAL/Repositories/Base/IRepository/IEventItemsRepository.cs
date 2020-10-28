using Core.Entities.Base;
using System.Collections.Generic;

namespace DAL.Repositories.Base.IRepository
{
    public interface IEventItemsRepository:IRepository<EventItem,int>
    {
        IEnumerable<EventItem> GetByEventId(int eventId);
        bool GetExists(string name, int eventId);
        List<int> Remove(int eventId);
    }
}
