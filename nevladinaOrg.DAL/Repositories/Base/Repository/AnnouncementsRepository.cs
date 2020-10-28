using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System.Linq;

namespace DAL.Repositories.Base.Repository
{
    public class AnnouncementsRepository : Repository<Announcement, int>, IAnnouncementsRepository
    {

        public AnnouncementsRepository(NevladinaOrgContext context) : base(context)
        {
        }

        public bool GetExists(string content)
        {
            return Context.Announcements.Any(x => x.Content == content && !x.IsDeleted);
        }
        public int Remove(int announcementId)
        {
            var id = Context.Announcements.Where(x => x.Id == announcementId).Select(x=>x.Id).FirstOrDefault();
            var announcement= Context.Announcements.Where(x => x.Id == announcementId).FirstOrDefault();
            Context.Announcements.Remove(announcement);
            return id;
        }
    }
}
