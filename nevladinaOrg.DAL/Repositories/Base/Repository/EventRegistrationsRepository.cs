using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.Base.Repository
{
    public class EventRegistrationsRepository :Repository<EventRegistration,int>,IEventRegistrationsRepository
    {
        public EventRegistrationsRepository(NevladinaOrgContext context) : base(context) { }
        public IEnumerable<EventRegistration> GetByEventId(int id)
        {
            return Context.EventRegistrations.Where(x => !x.IsDeleted && x.EventId == id);
        }
        public bool GetExists(int eventId, int userId)
        {
            return Context.EventRegistrations.Any(x => !x.IsDeleted && x.EventId == eventId && x.UserId == userId);
        }
        public List<int> Remove(int eventId)
        {
            var removedIds = Context.EventRegistrations.Where(x => x.EventId == eventId).Select(x => x.Id).ToList();
            var eventRegistrations = Context.EventRegistrations.Where(x => x.EventId == eventId);
            Context.EventRegistrations.RemoveRange(eventRegistrations);
            return removedIds;
        }
    }
}
