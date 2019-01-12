using Core.Entities.Base;

namespace DAL.Repositories.Base.IRepository
{
    public interface IPersonDetailsRepository:IRepository<PersonDetail,int>
    {
        int Remove(int userId);
    }
}
