using Core.Entities.Base;
using System;
using System.Collections.Generic;

namespace DAL.Repositories.Base.IRepository
{
    public interface IEventDocumentsRepository:IRepository<EventDocument,int>
    {
        List<int> Remove(int eventId);
        IEnumerable<EventDocument> GetByEventId(int eventId);
        EventDocument GetByName(string fileName, int eventId);
        EventDocument GetByStreamId(Guid streamId, int eventId);
    }
}
