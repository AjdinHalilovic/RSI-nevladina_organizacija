using Core.Entities.Base;
using System.Collections.Generic;

namespace DAL.Repositories.Base.IRepository
{
    public interface IEventRegistrationsRepository : IRepository<EventRegistration,int>
    {
        IEnumerable<EventRegistration> GetByEventId(int id);
        bool GetExists(int eventId, int userId);
        List<int> Remove(int eventId);
    }
}
