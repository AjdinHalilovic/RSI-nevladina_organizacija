using Core.Entities.Base;
using System.Collections.Generic;

namespace DAL.Repositories.Base.IRepository
{
    public interface IAnnouncementsRepository:IRepository<Announcement,int>
    {
        bool GetExists(string content);
        int Remove(int announcementId);

    }
}
