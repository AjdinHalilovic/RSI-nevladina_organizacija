using Core.Entities.Base.DTO;
using System.Collections.Generic;

namespace DAL.Repositories.Base.IRepository.DTO
{
    public interface IEventsDTORepository:IRepository<EventDTO,int>
    {
        EventDTO GetByEventId(int eventId);
        IEnumerable<EventDTO> GetByUserId(int userId);
    }
}
