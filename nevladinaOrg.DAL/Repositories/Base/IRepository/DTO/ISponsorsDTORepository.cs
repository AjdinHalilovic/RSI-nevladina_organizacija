using Core.Entities.Base.DTO;
using System.Collections.Generic;

namespace DAL.Repositories.Base.IRepository.DTO
{
    public interface ISponsorsDTORepository:IRepository<SponsorDTO,int>
    {
        IEnumerable<SponsorDTO> GetByEventId(int id);
    }
}
