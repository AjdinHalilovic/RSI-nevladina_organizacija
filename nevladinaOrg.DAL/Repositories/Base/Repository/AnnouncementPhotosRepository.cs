using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.Base.Repository
{
    public class AnnouncementPhotosRepository:Repository<AnnouncementPhoto,int>,IAnnouncementPhotosRepository
    {

        public AnnouncementPhotosRepository(NevladinaOrgContext context) : base(context)
        {
        }

        public IEnumerable<AnnouncementPhoto> GetPhotosByAnnouncementId(int announcementId)
        {
            return Context.AnnouncementPhotos.Where(x => !x.IsDeleted && x.AnnouncementId == announcementId); ;
        }
        public List<int> Remove(int announcementId)
        {
            var removedIds = Context.AnnouncementPhotos.Where(x => x.AnnouncementId == announcementId).Select(x => x.Id).ToList();
            var announcementPhotos = Context.AnnouncementPhotos.Where(x => x.AnnouncementId == announcementId);
            Context.AnnouncementPhotos.RemoveRange(announcementPhotos);
            return removedIds;
        }
        public IEnumerable<AnnouncementPhoto> GetByAnnouncementId(int id)
        {
            return Context.AnnouncementPhotos.Where(x => !x.IsDeleted && x.AnnouncementId == id);
        }
        public AnnouncementPhoto GetByName(string fileName, int announcementId)
        {
            return Context.AnnouncementPhotos.Where(x => !x.IsDeleted && x.Name == fileName && x.AnnouncementId == announcementId).FirstOrDefault();
        }
    }
}
