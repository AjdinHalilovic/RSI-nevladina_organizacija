using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repositories.Base.Repository
{
    public class AnnouncementUsersRepository:Repository<AnnouncementUser,int>,IAnnouncementUsersRepository
    {

        public AnnouncementUsersRepository(NevladinaOrgContext context) : base(context)
        {
        }

        public List<int> UserIdsByAnnouncementId(int Id)
        {
            return Context.AnnouncementUsers.Where(r => !r.IsDeleted && r.AnnouncementId == Id).Select(rf => rf.UserId).ToList();
        }
        public List<int> Remove(int announcementId)
        {
            var Ids = Context.AnnouncementUsers.Where(x => !x.IsDeleted && x.AnnouncementId == announcementId).Select(x => x.Id).ToList();
            Context.AnnouncementUsers.RemoveRange(Context.AnnouncementUsers.Where(x => !x.IsDeleted && x.AnnouncementId == announcementId));
            return Ids;
        }
        public List<int> RemoveWithoutCreator(int announcementId,int userId)
        {
            var Ids = Context.AnnouncementUsers.Where(x => !x.IsDeleted && x.AnnouncementId == announcementId && x.UserId!=userId).Select(x => x.Id).ToList();
            Context.AnnouncementUsers.RemoveRange(Context.AnnouncementUsers.Where(x => !x.IsDeleted && x.AnnouncementId == announcementId && x.UserId != userId));
            return Ids;
        }
        public AnnouncementUser GetByUserId(int userId, int announcementId)
        {
            return Context.AnnouncementUsers.SingleOrDefault(r => !r.IsDeleted && r.AnnouncementId == announcementId && r.UserId == userId);
        }


        //korekcija dole
        public IEnumerable<AnnouncementUser> GetAnnouncementUsers(int loggedUser, int announcementId)
        {
            return Context.AnnouncementUsers.Where(x => !x.IsDeleted && x.AnnouncementId == announcementId && x.UserId != loggedUser);
        }
        public void SetAnnouncementReviews(int loggedUser, int announcementId, bool value = false)
        {
            var reviews = Context.AnnouncementUsers
                .Where(x => !x.IsDeleted && x.AnnouncementId == announcementId && x.UserId != loggedUser);
            foreach (var review in reviews)
            {
                review.Seen = value;
            }
            Context.Set<AnnouncementUser>().UpdateRange(reviews);

        }
        public AnnouncementUser GetReview(int userId, int announcementId)
        {
            return Context.AnnouncementUsers.Where(x => !x.IsDeleted && x.AnnouncementId == announcementId && x.UserId == userId && x.OccurredAt==null && x.Seen==false).FirstOrDefault();
        }
    }
}
