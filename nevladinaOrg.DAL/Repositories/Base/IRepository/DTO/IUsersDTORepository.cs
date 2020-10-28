using Core.Entities.Base.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories.Base.IRepository.DTO
{
    public interface IUsersDTORepository : IRepository<UserDTO, int>
    {
        IEnumerable<UserDTO> GetByUserId(int userId);
        Task<IEnumerable<UserDTO>> GetByUserIdAsync(int userId);

        UserDTO GetByToken(Guid token, bool institutionUser);
        Task<UserDTO> GetByTokenAsync(Guid token, bool institutionUser);

    }
}
