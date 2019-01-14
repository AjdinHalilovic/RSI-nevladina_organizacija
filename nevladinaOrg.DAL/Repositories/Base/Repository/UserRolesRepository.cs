using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repositories.Base.Repository
{
    public class UserRolesRepository:Repository<UserRole,int>,IUserRolesRepository
    {
        public UserRolesRepository(NevladinaOrgContext context) : base(context) { }

        public IEnumerable<UserRole> GetByInstitutionUserId(int institutionUserId)
        {
            return Context.UserRoles.Where(x => !x.IsDeleted && x.InstitutionUserId == institutionUserId);
        }
        public IEnumerable<UserRole> GetByOrganizationInstitutionUserId(int organizationInstitutionUserId)
        {
            return Context.UserRoles.Where(x => !x.IsDeleted && x.OrganizationInstitutionUserId == organizationInstitutionUserId);
        }
    }
}
