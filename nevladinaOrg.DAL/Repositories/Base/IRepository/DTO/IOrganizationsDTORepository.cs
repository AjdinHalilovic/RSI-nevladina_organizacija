using Core.Entities.Base.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories.Base.IRepository.DTO
{
    public interface IOrganizationsDTORepository : IRepository<OrganizationDTO, int>
    {
        IEnumerable<OrganizationDTO> GetByParentId(int OrganizationParentId);
    }
}
