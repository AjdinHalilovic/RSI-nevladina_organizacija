using Core.Entities.Base.DTO;
using System;
using System.Collections.Generic;

namespace DAL.Repositories.Base.IRepository.DTO
{
    public interface IPersonsDTORepository:IRepository<PersonDTO,int>
    {
        IEnumerable<PersonDTO> GetByUserIds(List<int> userIds);
    }
}
