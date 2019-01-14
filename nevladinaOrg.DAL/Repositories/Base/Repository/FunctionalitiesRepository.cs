using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories.Base.Repository
{
    public class FunctionalitiesRepository : Repository<Functionality, int>, IFunctionalitiesRepository
    {
        public FunctionalitiesRepository(NevladinaOrgContext context) : base(context)
        {
        }

        public IEnumerable<Functionality> GetByInstitutionUserId(int institutionUserId)
        {
            //TO-DO Function
            var sql = "select t1.*\r\nfrom \"Functionalities\" as t1\r\njoin \"RoleFunctionalities\" as t2 on t1.\"Id\" = t2.\"FunctionalityId\"\r\njoin \"Roles\" as t3 on t2.\"RoleId\" = t3.\"Id\"\r\njoin \"UserRoles\" as t4 on t3.\"Id\" = t4.\"RoleId\"\r\nwhere t4.\"InstitutionUserId\" = @institutionUserId and t4.\"OrganizationInstitutionUserId\" is NULL";

            return DbConnection.Query<Functionality>(sql, new { institutionUserId });
        }
        public async Task<IEnumerable<Functionality>> GetByInstitutionUserIdAsync(int institutionUserId)
        {
            //TO-DO Function
            var sql = "select t1.*\r\nfrom \"Functionalities\" as t1\r\njoin \"RoleFunctionalities\" as t2 on t1.\"Id\" = t2.\"FunctionalityId\"\r\njoin \"Roles\" as t3 on t2.\"RoleId\" = t3.\"Id\"\r\njoin \"UserRoles\" as t4 on t3.\"Id\" = t4.\"RoleId\"\r\nwhere t4.\"InstitutionUserId\" = @institutionUserId and t4.\"OrganizationInstitutionUserId\" is NULL";

            return await DbConnection.QueryAsync<Functionality>(sql, new { institutionUserId });
        }

        public IEnumerable<Functionality> GetByOrganizationInstitutionUserId(int organizationInstitutionUserId)
        {
            //TO-DO Function
            var sql = "select t1.*\r\nfrom \"Functionalities\" as t1\r\njoin \"RoleFunctionalities\" as t2 on t1.\"Id\" = t2.\"FunctionalityId\"\r\njoin \"Roles\" as t3 on t2.\"RoleId\" = t3.\"Id\"\r\njoin \"UserRoles\" as t4 on t3.\"Id\" = t4.\"RoleId\"\r\nwhere t4.\"OrganizationInstitutionUserId\" = @organizationInstitutionUserId";

            return DbConnection.Query<Functionality>(sql, new { organizationInstitutionUserId });
        }
        public async Task<IEnumerable<Functionality>> GetByOrganizationInstitutionUserIdAsync(int organizationInstitutionUserId)
        {
            //TO-DO Function
            var sql = "select t1.*\r\nfrom \"Functionalities\" as t1\r\njoin \"RoleFunctionalities\" as t2 on t1.\"Id\" = t2.\"FunctionalityId\"\r\njoin \"Roles\" as t3 on t2.\"RoleId\" = t3.\"Id\"\r\njoin \"UserRoles\" as t4 on t3.\"Id\" = t4.\"RoleId\"\r\nwhere t4.\"OrganizationInstitutionUserId\" = @organizationInstitutionUserId";

            return await DbConnection.QueryAsync<Functionality>(sql, new { organizationInstitutionUserId });
        }

        public IEnumerable<Functionality> GetByRoleId(int roleId)
        {
            var functionalityIds = Context.RoleFunctionalities.Where(rf => !rf.IsDeleted && rf.RoleId == roleId).Select(rf => rf.FunctionalityId).ToList();

            return Context.Functionalities.Where(f => !f.IsDeleted && functionalityIds.Contains(f.Id));
        }
        public IEnumerable<Functionality> GetByUserId(int userId)
        {
            var institutionOrganizationUser = Context.InstitutionUsers.Where(x => !x.IsDeleted && x.UserId == userId).OrderByDescending(x=>x.LastLogin).FirstOrDefault();
            var roleIds = Context.UserRoles.Where(ur => !ur.IsDeleted && ur.InstitutionUserId == institutionOrganizationUser.Id).Select(ur => ur.RoleId).ToList();
            var functionalityIds = Context.RoleFunctionalities.Where(rf => !rf.IsDeleted && roleIds.Contains(rf.RoleId)).Select(rf => rf.FunctionalityId).ToList();

            return Context.Functionalities.Where(f => !f.IsDeleted && functionalityIds.Contains(f.Id));
        }
    }
}