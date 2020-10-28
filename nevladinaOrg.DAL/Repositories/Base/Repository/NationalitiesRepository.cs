using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System.Linq;

namespace DAL.Repositories.Base.Repository
{
    public class NationalitiesRepository : Repository<Nationality, int>, INationalitiesRepository
    {
        public NationalitiesRepository(NevladinaOrgContext context) : base(context)
        {
        }

        public bool GetExists(string name)
        {
            return Context.Nationalities.Any(x => x.Name == name && !x.IsDeleted);
        }
    }
}
