using Core.Entities.Base;

namespace DAL.Repositories.Base.IRepository
{
    public interface ILecturesRepository:IRepository<Lecture,int>
    {
        int? Remove(int eventItemId);
    }
}
