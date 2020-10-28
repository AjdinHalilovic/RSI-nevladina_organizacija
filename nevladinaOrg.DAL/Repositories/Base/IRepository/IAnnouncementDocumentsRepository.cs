using Core.Entities.Base;
using System;
using System.Collections.Generic;

namespace DAL.Repositories.Base.IRepository
{
    public interface IAnnouncementDocumentsRepository:IRepository<AnnouncementDocument,int>
    {
        List<string> GetDocumentsNameByAnnouncementsId(int announcementId);
        IEnumerable<AnnouncementDocument> GetDocumentsByAnnouncementId(int Id);
        List<int> Remove(int AnnouncementId);
        AnnouncementDocument GetByStreamId(Guid streamId, int announcementId);
        IEnumerable<AnnouncementDocument> GetByAnnouncementId(int id);
    }
}
