using Core.Entities.Base;
using System.Collections.Generic;

namespace DAL.Repositories.Base.IRepository
{
    public interface IEventSponsorsRepository:IRepository<EventSponsor,int>
    {
        List<int> Remove(int eventId);
        bool GetExists(int sponsorId, int eventId);
    }
}
