using Core.Entities.Base;

namespace DAL.Repositories.Base.IRepository
{
    public interface ILecturersRepository:IRepository<Lecturer,int>
    {
        Lecturer GetByUserId(int id);
    }
}
