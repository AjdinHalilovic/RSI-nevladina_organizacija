using Core.Entities.Base;
using System;
using System.Collections.Generic;


namespace DAL.Repositories.Base.IRepository
{
    public interface IInstitutionUsersRepository : IRepository<InstitutionUser,int>
    {
        IEnumerable<InstitutionUser> GetByUserId(int userId);
        InstitutionUser GetByUserIdAndInstitutionId(int userId, int institutionId);
        void SetLastLogin(int Id, DateTime dateTime);
    }
}
