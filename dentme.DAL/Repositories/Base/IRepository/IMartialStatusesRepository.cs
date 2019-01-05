using Core.Entities.Base;

namespace DAL.Repositories.Base.IRepository
{
    public interface IMartialStatusesRepository : IRepository<MaritalStatus, int>
    {
        bool GetExists(string name);
    }
}
