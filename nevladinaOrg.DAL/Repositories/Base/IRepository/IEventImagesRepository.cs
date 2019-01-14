using Core.Entities.Base;
using System.Collections.Generic;

namespace DAL.Repositories.Base.IRepository
{
    public interface IEventImagesRepository:IRepository<EventImage,int>
    {
        List<int> Remove(int eventId);
        IEnumerable<EventImage> GetByEventId(int id);
        EventImage GetByName(string fileName, int eventId);
    }
}
