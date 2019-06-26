using Core.Entities.Base;
using System.Collections.Generic;

namespace DAL.Repositories.Base.IRepository
{
    public interface IAnnouncementPhotosRepository : IRepository<AnnouncementPhoto, int>
    {
        IEnumerable<AnnouncementPhoto> GetPhotosByAnnouncementId(int announcementId);
        List<int> Remove(int AnnouncementId);
        IEnumerable<AnnouncementPhoto> GetByAnnouncementId(int id);
        AnnouncementPhoto GetByName(string fileName, int announcementId);
    }
}
