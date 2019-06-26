using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.Base.Repository
{
    public class AnnouncementDocumentsRepository:Repository<AnnouncementDocument,int>,IAnnouncementDocumentsRepository
    {

        public AnnouncementDocumentsRepository(NevladinaOrgContext context) : base(context)
        {
        }

        public List<string> GetDocumentsNameByAnnouncementsId(int announcementId)
        {
            return Context.AnnouncementDocuments.Where(x => !x.IsDeleted && x.AnnouncementId == announcementId).Select(x => x.Name).ToList();
        }
        public IEnumerable<AnnouncementDocument> GetDocumentsByAnnouncementId(int Id)
        {
            return Context.AnnouncementDocuments.Where(x => !x.IsDeleted && x.AnnouncementId == Id);
        }
        public List<int> Remove(int announcementId)
        {
            var removedIds = Context.AnnouncementDocuments.Where(x => x.AnnouncementId == announcementId).Select(x => x.Id).ToList();
            var announcementDocuments = Context.AnnouncementDocuments.Where(x => x.AnnouncementId == announcementId);
            Context.AnnouncementDocuments.RemoveRange(announcementDocuments);
            return removedIds;
        }
        public AnnouncementDocument GetByStreamId(Guid streamId, int announcementId)
        {
            return Context.AnnouncementDocuments.Where(x => !x.IsDeleted && x.StreamId == streamId && x.AnnouncementId == announcementId).FirstOrDefault();
        }
        public IEnumerable<AnnouncementDocument> GetByAnnouncementId(int id)
        {
            return Context.AnnouncementDocuments.Where(x => !x.IsDeleted && x.AnnouncementId == id);
        }
    }
}
