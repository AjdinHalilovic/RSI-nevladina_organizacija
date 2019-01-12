using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;

namespace DAL.Repositories.Base.Repository
{
    public class MembersRepository:Repository<Member,int>,IMembersRepository
    {
        public MembersRepository(NevladinaOrgContext context) : base(context) { }
    }
}
