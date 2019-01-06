using Core.Entities.Base.DTO;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.Base.IRepository.DTO
{
    public interface IEventItemsDTORepository:IRepository<EventItemDTO,int>
    {
        IEnumerable<EventItemDTO> GetByEventId(int eventId);
    }
}
