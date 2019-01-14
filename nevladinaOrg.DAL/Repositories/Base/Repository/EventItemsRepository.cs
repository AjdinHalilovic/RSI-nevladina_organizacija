using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.Base.Repository
{
    public class EventItemsRepository:Repository<EventItem,int>,IEventItemsRepository
    {
        public EventItemsRepository(NevladinaOrgContext context) : base(context) { }
        public IEnumerable<EventItem> GetByEventId(int eventId)
        {
            return Context.EventItems.Where(x => !x.IsDeleted && x.EventId == eventId);
        }
        public bool GetExists(string name,int eventId)
        {
            return Context.EventItems.Any(x => !x.IsDeleted && x.Name == name && x.EventId==eventId);
        }
        public List<int> Remove(int eventId)
        {
            var removedIds = Context.EventItems.Where(x =>!x.IsDeleted && x.EventId == eventId).Select(x => x.Id).ToList();
            var eventItems = Context.EventItems.Where(x =>!x.IsDeleted && x.EventId == eventId);
            Context.EventItems.RemoveRange(eventItems);
            return removedIds;
        }
    }
}
