using Core.Entities.Base;
using System.Collections.Generic;

namespace DAL.Repositories.Base.IRepository
{
    public interface IUsersRepository: IRepository<User,int>
    {
        User GetByUsername(string username);

        void UpdateCultureName(int id, string culture);
        int Remove(int Id);
        bool GetExistsByUsername(string username, string email);
    }
}
