using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System.Linq;

namespace DAL.Repositories.Base.Repository
{
    public class EventsRepository:Repository<Event,int>,IEventsRepository
    {
        public EventsRepository(NevladinaOrgContext context) : base(context) { }
        public bool GetExists(string name)
        {
            return Context.Events.Any(x => !x.IsDeleted && x.Name == name);
        }
    }
}
