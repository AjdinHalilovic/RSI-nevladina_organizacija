using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System.Linq;

namespace DAL.Repositories.Base.Repository
{
    public class LecturersRepository:Repository<Lecturer,int>,ILecturersRepository
    {
        public LecturersRepository(NevladinaOrgContext context) : base(context) { }
        public Lecturer GetByUserId(int id)
        {
            return Context.Lecturers.Where(x => x.UserId == id).FirstOrDefault();
        }
    }
}
