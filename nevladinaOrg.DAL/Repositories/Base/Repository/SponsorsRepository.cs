using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System.Linq;

namespace DAL.Repositories.Base.Repository
{
    public class SponsorsRepository:Repository<Sponsor,int>,ISponsorsRepository
    {
        public SponsorsRepository(NevladinaOrgContext context) : base(context) { }
        public bool GetExists(string name)
        {
            return Context.Sponsors.Any(x => !x.IsDeleted && x.Name == name);
        }
    }
}
