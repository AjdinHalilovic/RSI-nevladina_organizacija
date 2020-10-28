using Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories.Base.IRepository
{
    public interface IOrganizationInstitutionUsersRepository : IRepository<OrganizationInstitutionUser, int>
    {
        IEnumerable<OrganizationInstitutionUser> GetByInstitutionUserIds(int[] institutionUserIds);
        IEnumerable<OrganizationInstitutionUser> GetByInstitutionUserIds(string institutionUserIds);
        OrganizationInstitutionUser GetByInstitutionUserId(int institutionUserId);
        int? Remove(int institutionUserId);

        void SetLastLogin(int Id, DateTime dateTime);
    }
}
