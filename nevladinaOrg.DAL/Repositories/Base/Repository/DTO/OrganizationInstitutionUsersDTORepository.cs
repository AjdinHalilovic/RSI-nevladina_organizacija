using Core.Entities.Base.DTO;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repositories.Base.Repository.DTO
{
    public class OrganizationInstitutionUsersDTORepository:Repository<OrganizationInstitutionUserDTO,int>,IOrganizationInstitutionUsersDTORepository
    {
        public OrganizationInstitutionUsersDTORepository(NevladinaOrgContext context):base(context) {}
        public IEnumerable<OrganizationInstitutionUserDTO> GetAllByOrganizationId(int organizationId)
        {
            var organizationInstitutionUsers = Context.OrganizationInstitutionUsers.Include(x => x.InstitutionUser)
                .Where(x => !x.IsDeleted && x.OrganizationId == organizationId)
                .Select(x => new OrganizationInstitutionUserDTO
                {
                    Id = x.Id,
                    OrganizationId = x.OrganizationId,
                    InstitutionUserId = x.InstitutionUserId,
                    UserId = x.InstitutionUser.UserId,
                    Active = x.Active,
                    Employed = x.Employed,
                }).ToList();
            var members = Context.Members.Where(x => organizationInstitutionUsers.Select(y => y.Id).Contains(x.Id)).Select(x => x.Id);
            foreach (var organizationInstitutionUser in organizationInstitutionUsers)
            {
                organizationInstitutionUser.Member = members.Contains(organizationInstitutionUser.Id) ? true : false;
            }
            return organizationInstitutionUsers;
        }
    }
}
