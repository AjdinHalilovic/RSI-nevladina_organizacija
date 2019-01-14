using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.Base.Repository
{
    public class EventUsersRepository:Repository<EventUser,int>,IEventUsersRepository
    {
        public EventUsersRepository(NevladinaOrgContext context) : base(context) { }
        public IEnumerable<EventUser> GetEventUsers(int loggedUser, int eventId)
        {
            return Context.EventUsers.Where(x => !x.IsDeleted && x.EventId == eventId && x.UserId != loggedUser);
        }
        public void SetEventReviews(int loggedUser, int eventId, bool value = false)
        {
            var reviews = Context.EventUsers
                .Where(x => !x.IsDeleted && x.EventId == eventId && x.UserId != loggedUser)
                .Select(x => new EventUser { Id = x.Id, UserId = x.UserId, EventId = x.EventId, OccurredAt = x.OccurredAt, Seen = value, IsDeleted = false });
            Context.Set<EventUser>().UpdateRange(reviews);
            Context.SaveChanges();
        }
        public EventUser GetReview(int userId, int eventId)
        {
            return Context.EventUsers.Where(x => !x.IsDeleted && x.EventId == eventId && x.UserId == userId).FirstOrDefault();
        }
        public List<int> Remove(int eventId)
        {
            var Ids = Context.EventUsers.Where(x => !x.IsDeleted && x.EventId == eventId).Select(x => x.Id).ToList();
            Context.EventUsers.RemoveRange(Context.EventUsers.Where(x => !x.IsDeleted && x.EventId == eventId));
            return Ids;
        }
    }
}
