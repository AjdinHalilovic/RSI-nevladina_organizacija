using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.Base.Repository
{
    public class EventDocumentsRepository:Repository<EventDocument,int>,IEventDocumentsRepository
    {
        public EventDocumentsRepository(NevladinaOrgContext context) : base(context) { }
        public List<int> Remove(int eventId)
        {
            var removedIds = Context.EventDocuments.Where(x => x.EventId == eventId).Select(x => x.Id).ToList();
            var eventDocuments = Context.EventDocuments.Where(x => x.EventId == eventId);
            Context.EventDocuments.RemoveRange(eventDocuments);
            return removedIds;
        }
        public IEnumerable<EventDocument> GetByEventId(int eventId)
        {
            return Context.EventDocuments.Where(x => !x.IsDeleted && x.EventId == eventId);
        }
        public EventDocument GetByName(string fileName,int eventId)
        {
            return Context.EventDocuments.Where(x => !x.IsDeleted && x.Name == fileName && x.EventId==eventId).FirstOrDefault();
        }
        public EventDocument GetByStreamId(Guid streamId, int eventId)
        {
            return Context.EventDocuments.Where(x => !x.IsDeleted && x.StreamId == streamId && x.EventId == eventId).FirstOrDefault();
        }
    }
}
