using Core.Entities.Base;
using System.Collections.Generic;

namespace DAL.Repositories.Base.IRepository
{
    public interface IAnnouncementOrganizationsRepository:IRepository<AnnouncementOrganization,int>
    {
        List<int> OrganizationIdsByAnnouncementId(int Id);
        AnnouncementOrganization GetByOrganizationId(int organizationId, int announcementId);
        IEnumerable<AnnouncementOrganization> GetOrganizationsByAnnouncementId(int Id);
        List<int> Remove(int AnnouncementId);

    }
}
