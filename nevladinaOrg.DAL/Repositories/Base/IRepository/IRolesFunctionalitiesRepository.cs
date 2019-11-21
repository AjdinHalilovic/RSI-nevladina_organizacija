using Core.Entities.Base;
using System.Collections.Generic;
namespace DAL.Repositories.Base.IRepository
{
    public interface IRolesFunctionalitiesRepository : IRepository<RoleFunctionality, int>
    {
        List<int> FunctionalityIdsByRoleId(int Id);
        RoleFunctionality GetByFunctionalityId(int Fid, int Rid);
        IEnumerable<RoleFunctionality> GetFunctionalitiesByRoleId(int Id);
    }
}