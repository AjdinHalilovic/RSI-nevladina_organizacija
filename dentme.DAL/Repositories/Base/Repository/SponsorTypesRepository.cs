using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;

namespace DAL.Repositories.Base.Repository
{
    public class SponsorTypesRepository:Repository<SponsorType,int>,ISponsorTypesRepository
    {
        public SponsorTypesRepository(NevladinaOrgContext context) : base(context) { }
    }
}
