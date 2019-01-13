using Core.Entities.Base;
using System.Collections.Generic;


namespace DAL.Repositories.Base.IRepository
{
    public interface IRolesRepository:IRepository<Role,int>
    {
        bool GetExists(string name);
        IEnumerable<Role> GetByOrganizationInstitutionUserId(int organizationInstitutionUserId);
        IEnumerable<Role> GetByInstitutionUserId(int institutionUserId);

    }
}
