using Core.Entities.Base;
using System.Collections.Generic;

namespace DAL.Repositories.Base.IRepository
{
    public interface IUserRolesRepository:IRepository<UserRole,int>
    {
        IEnumerable<UserRole> GetByInstitutionUserId(int institutionUserId);
        IEnumerable<UserRole> GetByOrganizationInstitutionUserId(int organizationInstitutionUserId);
    }
}
