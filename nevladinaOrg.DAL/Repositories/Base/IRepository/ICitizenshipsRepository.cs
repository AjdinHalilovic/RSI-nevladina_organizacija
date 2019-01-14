using Core.Entities.Base;

namespace DAL.Repositories.Base.IRepository
{
    public interface ICitizenshipsRepository : IRepository<Citizenship, int>
    {
        bool GetExists(string name);
    }
}
