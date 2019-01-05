using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System.Linq;

namespace DAL.Repositories.Base.Repository
{
    public class CitizenshipsRepository : Repository<Citizenship, int>, ICitizenshipsRepository
    {
        public CitizenshipsRepository(NevladinaOrgContext context) : base(context)
        {
        }

        public bool GetExists(string name)
        {
            return Context.Citizenships.Any(x => x.Name == name && x.IsDeleted == false);
        }
    }
}
