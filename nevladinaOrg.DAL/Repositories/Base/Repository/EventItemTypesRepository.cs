using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;

namespace DAL.Repositories.Base.Repository
{
    public class EventItemTypesRepository:Repository<EventItemType,int>,IEventItemTypesRepository
    {
        public EventItemTypesRepository(NevladinaOrgContext context) : base(context) { }
    }
}
