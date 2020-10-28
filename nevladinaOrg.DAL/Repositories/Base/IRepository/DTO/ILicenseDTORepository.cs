using Core.Entities.Base.DTO;
using System.Collections.Generic;

namespace DAL.Repositories.Base.IRepository.DTO
{
    public interface ILicenseDTORepository:IRepository<LicenseDTO,int>
    {
        IEnumerable<LicenseDTO> GetByMemberId(int Id);
    }
}
