using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.Base.Repository
{
    public class AnnouncementOrganizationsRepository:Repository<AnnouncementOrganization,int>,IAnnouncementOrganizationsRepository
    {

        public AnnouncementOrganizationsRepository(NevladinaOrgContext context) : base(context)
        {
        }

        public List<int> OrganizationIdsByAnnouncementId(int Id)
        {
            return Context.AnnouncementOrganizations.Where(r => !r.IsDeleted && r.AnnouncementId == Id).Select(rf => rf.OrganizationId).ToList();
        }
        public AnnouncementOrganization GetByOrganizationId(int organizationId, int announcementId)
        {
            return Context.AnnouncementOrganizations.SingleOrDefault(r => !r.IsDeleted && r.AnnouncementId == announcementId && r.OrganizationId== organizationId);
        }
        public IEnumerable<AnnouncementOrganization> GetOrganizationsByAnnouncementId(int Id)
        {
            return Context.AnnouncementOrganizations.Where(r => !r.IsDeleted && r.AnnouncementId == Id);
        }
        public List<int> Remove(int announcementId)
        {
            var removedIds = Context.AnnouncementOrganizations.Where(x => x.AnnouncementId == announcementId).Select(x => x.Id ).ToList();
            var announcementOrganizations= Context.AnnouncementOrganizations.Where(x => x.AnnouncementId == announcementId);
            Context.AnnouncementOrganizations.RemoveRange(announcementOrganizations);
            return removedIds;
        }
    }
}
