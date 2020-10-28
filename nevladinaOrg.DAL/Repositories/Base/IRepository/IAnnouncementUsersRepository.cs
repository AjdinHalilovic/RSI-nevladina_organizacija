using Core.Entities.Base;
using System.Collections.Generic;

namespace DAL.Repositories.Base.IRepository
{
    public interface IAnnouncementUsersRepository:IRepository<AnnouncementUser,int>
    {
        List<int> UserIdsByAnnouncementId(int Id);
        List<int> Remove(int announcementId);
        AnnouncementUser GetByUserId(int userId, int announcementId);
        AnnouncementUser GetReview(int userId, int announcementId);
        IEnumerable<AnnouncementUser> GetAnnouncementUsers(int loggedUser, int announcementId);
        void SetAnnouncementReviews(int loggedUser, int announcementId, bool value = false);
        List<int> RemoveWithoutCreator(int announcementId, int userId);
    }
}
