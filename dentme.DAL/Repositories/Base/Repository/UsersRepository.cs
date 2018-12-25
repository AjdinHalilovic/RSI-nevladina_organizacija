using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System.Linq;

namespace DAL.Repositories.Base.Repository
{
    public class UsersRepository:Repository<User,int>,IUsersRepository
    {
        public UsersRepository(NevladinaOrgContext context) : base(context)
        {
        }


        public User GetByUsername(string username)
        {
            return Context.Users.FirstOrDefault(x => x.Username == username);
        }

        public void UpdateCultureName(int id, string culture)
        {
            var user = new User { Id = id, CultureName = culture };
            Context.Entry(user).Property(u => u.CultureName).IsModified = true;
        }
        public int Remove(int Id)
        {
            var user = Context.Users.FirstOrDefault(x => x.Id == Id);
            Context.Users.Remove(user);
            return user.Id;
        }
        public bool GetExistsByUsername(string username, string email)
        {
            return Context.Users.Any(x => x.Username == username && x.Email == email);
        }

    }
}
