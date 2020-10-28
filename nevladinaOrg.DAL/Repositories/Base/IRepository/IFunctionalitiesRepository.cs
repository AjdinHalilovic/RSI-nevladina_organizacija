using Core.Entities.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories.Base.IRepository
{
    public interface IFunctionalitiesRepository : IRepository<Functionality, int>
    {

        IEnumerable<Functionality> GetByRoleId(int roleId);

        IEnumerable<Functionality> GetByUserId(int userId);

        IEnumerable<Functionality> GetByInstitutionUserId(int institutionUserId);
        Task<IEnumerable<Functionality>> GetByInstitutionUserIdAsync(int institutionUserId);

        IEnumerable<Functionality> GetByOrganizationInstitutionUserId(int organizationInstitutionUserId);
        Task<IEnumerable<Functionality>> GetByOrganizationInstitutionUserIdAsync(int organizationInstitutionUserId);

    }
}