using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.Base.Repository
{
    public class AnnouncementTypesRepository : Repository<AnnouncementType, int>, IAnnouncementTypesRepository
    {

        public AnnouncementTypesRepository(NevladinaOrgContext context) : base(context)
        {
        }

        public override IEnumerable<AnnouncementType> GetAll() => Context.AnnouncementTypes.Where(x => x.IsDeleted == false);

        public bool GetExists(string name) => Context.AnnouncementTypes.Where(x => x.IsDeleted == false).Any(x => x.Name == name);
    }
}
