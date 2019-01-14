using Core.Entities.Base.DTO;
using System.Collections.Generic;

namespace DAL.Repositories.Base.IRepository.DTO
{
    public interface IInstitutionUsersDTORepository:IRepository<InstitutionUserDTO,int>
    {
        IEnumerable<InstitutionUserDTO> GetInstitutionUsersNotOrganizationUsersByInsitutionId(int institutionId);
        IEnumerable<InstitutionUserDTO> GetByInstitutionId(int institutionId);
    }
}
