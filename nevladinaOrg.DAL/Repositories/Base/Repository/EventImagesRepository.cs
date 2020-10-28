using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.Base.Repository
{
    public class EventImagesRepository : Repository<EventImage, int>, IEventImagesRepository
    {
        public EventImagesRepository(NevladinaOrgContext context) : base(context) { }
        public List<int> Remove(int eventId)
        {
            var eventImages = Context.EventImages.Where(x => x.EventId == eventId);
            Context.EventImages.RemoveRange(eventImages);

            return eventImages.Select(x => x.Id).ToList();
        }
        public IEnumerable<EventImage> GetByEventId(int id)
        {
            return Context.EventImages.Where(x => !x.IsDeleted && x.EventId == id);
        }
        public EventImage GetByName(string fileName, int eventId)
        {
            return Context.EventImages.FirstOrDefault(x => !x.IsDeleted && x.Name == fileName && x.EventId == eventId);
        }
    }
}
