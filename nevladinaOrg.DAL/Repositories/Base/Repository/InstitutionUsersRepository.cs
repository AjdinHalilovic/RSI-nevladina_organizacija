using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.Base.Repository
{
    public class InstitutionUsersRepository : Repository<InstitutionUser, int>, IInstitutionUsersRepository
    {
        public InstitutionUsersRepository(NevladinaOrgContext context)
            : base(context)
        {
        }
        public IEnumerable<InstitutionUser> GetByUserId(int userId)
        {
            return Context.InstitutionUsers.Where(x => !x.IsDeleted && x.UserId == userId).OrderByDescending(x => x.LastLogin);
        }

        public void SetLastLogin(int Id, DateTime dateTime)
        {
            var user = Context.InstitutionUsers.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
            user.LastLogin = dateTime;
            Update(user);
        }

        public int Remove(int Id)
        {
            var institutionUser = Context.InstitutionUsers.FirstOrDefault(x => x.Id == Id);
            Context.InstitutionUsers.Remove(institutionUser);
            return institutionUser.Id;
        }
        public InstitutionUser GetByUserIdAndInstitutionId(int userId,int institutionId)
        {
            return Context.InstitutionUsers.FirstOrDefault(x => !x.IsDeleted && x.UserId == userId && x.InstitutionId == institutionId);
        }
    }
}
