using Core.Entities.Base.DTO;
using System.Collections.Generic;

namespace DAL.Repositories.Base.IRepository.DTO
{
    public interface IOrganizationInstitutionUsersDTORepository:IRepository<OrganizationInstitutionUserDTO,int>
    {
        IEnumerable<OrganizationInstitutionUserDTO> GetAllByOrganizationId(int organizationId);
    }
}
