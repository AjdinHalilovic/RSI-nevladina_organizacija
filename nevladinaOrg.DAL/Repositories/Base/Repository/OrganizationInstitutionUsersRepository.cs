using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repositories.Base.Repository
{
    public class OrganizationInstitutionUsersRepository : Repository<OrganizationInstitutionUser, int>, IOrganizationInstitutionUsersRepository
    {
        public OrganizationInstitutionUsersRepository(NevladinaOrgContext context) : base(context)
        {
        }

        public IEnumerable<OrganizationInstitutionUser> GetByInstitutionUserIds(int[] institutionUserIds)
        {
            return Context.OrganizationInstitutionUsers.Where(x => institutionUserIds.Contains(x.Id));
        }
        public OrganizationInstitutionUser GetByInstitutionUserId(int institutionUserId)
        {
            return Context.OrganizationInstitutionUsers.FirstOrDefault(x => x.InstitutionUserId == institutionUserId);
        }

        public IEnumerable<OrganizationInstitutionUser> GetByInstitutionUserIds(string institutionUserIds)
        {
            int[] _institutionUserIds = Array.ConvertAll(institutionUserIds.Split(','), Convert.ToInt32);

            return Context.OrganizationInstitutionUsers.Where(x => _institutionUserIds.Contains(x.Id));
        }

        public void SetLastLogin(int Id, DateTime dateTime)
        {
            var user = Context.OrganizationInstitutionUsers.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
            user.LastLogin = dateTime;
            Update(user);
        }
        public int? Remove(int institutionUserId)
        {
            var organizationInstitutionUser = Context.OrganizationInstitutionUsers.FirstOrDefault(x => x.InstitutionUserId == institutionUserId);
            if (organizationInstitutionUser != null)
            {
                Context.OrganizationInstitutionUsers.Remove(organizationInstitutionUser);
                return organizationInstitutionUser.Id;
            }
            return null;
        }
    }
}
