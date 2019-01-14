using System.Collections.Generic;
using System.Linq;
using Core.Entities.Base.DTO;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository.DTO;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Base.Repository.DTO
{
    public class InstitutionUsersDTORepository : Repository<InstitutionUserDTO, int>, IInstitutionUsersDTORepository
    {
        public InstitutionUsersDTORepository(NevladinaOrgContext context) : base(context)
        {
        }

        public IEnumerable<InstitutionUserDTO> GetInstitutionUsersNotOrganizationUsersByInsitutionId(int institutionId)
        {
            var organizationUserIds = Context.OrganizationInstitutionUsers
                .Include(x => x.InstitutionUser)
                .Where(x => !x.IsDeleted && x.InstitutionUser.InstitutionId == institutionId)
                .Select(x => x.InstitutionUserId).ToList();

            var institutionUsers= Context.InstitutionUsers.Where(x => !x.IsDeleted && x.InstitutionId==institutionId && !organizationUserIds.Contains(x.Id))
                .Select(x => new InstitutionUserDTO
                {
                    Id = x.Id,
                    InstitutionId = x.InstitutionId,
                    UserId = x.UserId,
                    Active = x.Active
                }).ToList();
            return institutionUsers;
        }
        public IEnumerable<InstitutionUserDTO> GetByInstitutionId(int institutionId)
        {
            var institutionUsers = Context.InstitutionUsers.Where(x => !x.IsDeleted && x.InstitutionId == institutionId)
                            .Select(x => new InstitutionUserDTO
                            {
                                Id = x.Id,
                                InstitutionId = x.InstitutionId,
                                UserId = x.UserId,
                                Active = x.Active
                            }).ToList();
            return institutionUsers;
        }
    }
}
