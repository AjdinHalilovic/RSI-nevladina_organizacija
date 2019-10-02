using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.Base.Repository
{
    public class RolesFunctionalitiesRepository:Repository<RoleFunctionality,int>,IRolesFunctionalitiesRepository
    {
        public RolesFunctionalitiesRepository(NevladinaOrgContext context) : base(context) { }
        public List<int> FunctionalityIdsByRoleId(int Id)
        {
            return Context.RoleFunctionalities.Where(r => !r.IsDeleted && r.RoleId == Id).Select(rf => rf.FunctionalityId).ToList();
        }
        public RoleFunctionality GetByFunctionalityId(int Fid, int Rid)
        {
            return Context.RoleFunctionalities.SingleOrDefault(r => !r.IsDeleted && r.RoleId == Rid && r.FunctionalityId == Fid);
        }
        public IEnumerable<RoleFunctionality> GetFunctionalitiesByRoleId(int Id)
        {
            return Context.RoleFunctionalities.Where(r => !r.IsDeleted && r.RoleId == Id);
        }
    }
}
