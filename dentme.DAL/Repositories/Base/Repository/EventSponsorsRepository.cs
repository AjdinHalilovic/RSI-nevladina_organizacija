using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.Base.Repository
{
    public class EventSponsorsRepository:Repository<EventSponsor,int>,IEventSponsorsRepository
    {
        public EventSponsorsRepository(NevladinaOrgContext context) : base(context) { }
        public List<int> Remove(int eventId)
        {
            var removedIds = Context.EventSponsors.Where(x => x.EventId == eventId).Select(x => x.Id).ToList();
            var eventSponsors = Context.EventSponsors.Where(x => x.EventId == eventId);
            Context.EventSponsors.RemoveRange(eventSponsors);
            return removedIds;
        }
        public bool GetExists(int sponsorId,int eventId)
        {
            return Context.EventSponsors.Any(x => !x.IsDeleted && x.SponsorId == sponsorId && x.EventId==eventId);
        }
    }
}
