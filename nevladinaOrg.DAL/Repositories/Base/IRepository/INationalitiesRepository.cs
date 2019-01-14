using Core.Entities.Base;

namespace DAL.Repositories.Base.IRepository
{
    public interface INationalitiesRepository : IRepository<Nationality, int>
    {
        bool GetExists(string name);
    }
}
