using Core.Entities.Base.DTO;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories.Base.Repository.DTO
{
    public class UsersDTORepository : Repository<UserDTO, int>, IUsersDTORepository
    {
        public UsersDTORepository(NevladinaOrgContext context) : base(context)
        {
        }

        public UserDTO GetByToken(Guid token, bool institutionUser)
        {
            string sql = string.Empty;

            if (institutionUser)
                sql = $"select ROW_NUMBER () OVER (ORDER BY t1.\"UserId\"), t1.\"Token\", t1.\"UserId\", t1.\"InstitutionId\", t2.\"OrganizationId\", t1.\"Employed\", t1.\"Active\"\r\nfrom \"InstitutionUsers\" as t1 \r\nleft join \"OrganizationInstitutionUsers\" as t2 on t1.\"Id\" = t2.\"InstitutionUserId\" and t2.\"IsDeleted\" = 0 \r\nwhere t1.\"Token\" = @token and t1.\"IsDeleted\" = 0";
            else
                sql = $"select ROW_NUMBER () OVER (ORDER BY t1.\"UserId\"), t2.\"Token\", t2.\"UserId\", t1.\"InstitutionId\", t2.\"OrganizationId\", t1.\"Employed\", t2.\"Active\"\r\nfrom \"InstitutionUsers\" as t1 \r\njoin \"OrganizationInstitutionUsers\" as t2 on t1.\"Id\" = t2.\"InstitutionUserId\" and t2.\"IsDeleted\" = 0 \r\nwhere t2.\"Token\" = @token and t1.\"IsDeleted\" = 0";

            return DbConnection.QueryFirstOrDefault<UserDTO>(sql, new { token });
        }
        public async Task<UserDTO> GetByTokenAsync(Guid token, bool institutionUser)
        {
            string sql = string.Empty;

            if (institutionUser)
                sql = $"select ROW_NUMBER () OVER (ORDER BY t1.\"UserId\"), t1.\"Token\", t1.\"UserId\", t1.\"InstitutionId\", t2.\"OrganizationId\", t1.\"Employed\", t1.\"Active\"\r\nfrom \"InstitutionUsers\" as t1 \r\nleft join \"OrganizationInstitutionUsers\" as t2 on t1.\"Id\" = t2.\"InstitutionUserId\" and t2.\"IsDeleted\" = 0 \r\nwhere t1.\"Token\" = @token and t1.\"IsDeleted\" = 0";
            else
                sql = $"select ROW_NUMBER () OVER (ORDER BY t1.\"UserId\"), t2.\"Token\", t2.\"UserId\", t1.\"InstitutionId\", t2.\"OrganizationId\", t1.\"Employed\", t2.\"Active\"\r\nfrom \"InstitutionUsers\" as t1 \r\njoin \"OrganizationInstitutionUsers\" as t2 on t1.\"Id\" = t2.\"InstitutionUserId\" and t2.\"IsDeleted\" = 0 \r\nwhere t2.\"Token\" = @token and t1.\"IsDeleted\" = 0";

            return await DbConnection.QueryFirstOrDefaultAsync<UserDTO>(sql, new { token });
        }

        public IEnumerable<UserDTO> GetByUserId(int userId)
        {
            var sql = $"select ROW_NUMBER () OVER (ORDER BY t1.\"UserId\") as \"Id\", COALESCE(t2.\"Token\", t1.\"Token\") as \"Token\", t1.\"UserId\", t1.\"Id\" as \"InstitutionUserId\", t1.\"InstitutionId\", t3.\"Id\" as \"MemberId\", t2.\"Employed\", t2.\"Id\" as \"OrganizationInstitutionUserId\", t2.\"OrganizationId\",  COALESCE(t2.\"LastLogin\", t1.\"LastLogin\") as \"LastLogin\" \r\nfrom \"InstitutionUsers\" as t1 \r\nleft join \"OrganizationInstitutionUsers\" as t2 on t1.\"Id\" = t2.\"InstitutionUserId\" and t2.\"IsDeleted\" = 0\r\nleft join \"Members\" as t3 on t2.\"Id\" = t3.\"Id\"\r\nwhere t1.\"UserId\" = @userId and t1.\"IsDeleted\" = 0";

            return DbConnection.QueryAsync<UserDTO>(sql, new { userId }).Result;
        }
        public async Task<IEnumerable<UserDTO>> GetByUserIdAsync(int userId)
        {
            var sql = $"select ROW_NUMBER () OVER (ORDER BY t1.\"UserId\") as \"Id\", COALESCE(t2.\"Token\", t1.\"Token\") as \"Token\", t1.\"UserId\", t1.\"Id\" as \"InstitutionUserId\", t1.\"InstitutionId\", t3.\"Id\" as \"MemberId\", t2.\"Employed\", t2.\"Id\" as \"OrganizationInstitutionUserId\", t2.\"OrganizationId\",  COALESCE(t2.\"LastLogin\", t1.\"LastLogin\") as \"LastLogin\" \r\nfrom \"InstitutionUsers\" as t1 \r\nleft join \"OrganizationInstitutionUsers\" as t2 on t1.\"Id\" = t2.\"InstitutionUserId\" and t2.\"IsDeleted\" = 0\r\nleft join \"Members\" as t3 on t2.\"Id\" = t3.\"Id\"\r\nwhere t1.\"UserId\" = @userId and t1.\"IsDeleted\" = 0";

            return await DbConnection.QueryAsync<UserDTO>(sql, new { userId });
        }

    }
}
