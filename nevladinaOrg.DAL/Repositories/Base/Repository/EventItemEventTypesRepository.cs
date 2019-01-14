using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.Base.Repository
{
    public class EventItemEventTypesRepository:Repository<EventItemEventType,int>,IEventItemEventTypesRepository
    {
        public EventItemEventTypesRepository(NevladinaOrgContext context) : base(context) { }
        public List<int> RemoveByEventItemId(int id)
        {
            var removedIds = Context.EventItemEventTypes.Where(x => !x.IsDeleted && x.EventItemId == id).Select(x => x.Id).ToList();
            Context.EventItemEventTypes.RemoveRange(Context.EventItemEventTypes.Where(x => !x.IsDeleted && x.EventItemId == id));
            return removedIds;
        }
        public IEnumerable<EventItemEventType> GetByEventItemId(int Id)
        {
            return Context.EventItemEventTypes.Where(x => !x.IsDeleted && x.EventItemId == Id);
        }
        public List<int> Remove(int eventItemId)
        {
            var removedIds = Context.EventItemEventTypes.Where(x => x.EventItemId == eventItemId).Select(x => x.Id).ToList();
            var eventItemEventTypes = Context.EventItemEventTypes.Where(x => x.EventItemId == eventItemId);
            Context.EventItemEventTypes.RemoveRange(eventItemEventTypes);
            return removedIds;
        }
    }
}
