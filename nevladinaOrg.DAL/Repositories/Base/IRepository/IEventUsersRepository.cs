using Core.Entities.Base;
using System.Collections.Generic;

namespace DAL.Repositories.Base.IRepository
{
    public interface IEventUsersRepository:IRepository<EventUser,int>
    {
        IEnumerable<EventUser> GetEventUsers(int loggedUser, int eventId);
        void SetEventReviews(int loggedUser, int eventId, bool value = false);
        EventUser GetReview(int userId, int eventId);
        List<int> Remove(int eventId);
    }
}
