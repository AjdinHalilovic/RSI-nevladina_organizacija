using Core.Entities.Base;

namespace DAL.Repositories.Base.IRepository
{
    public interface ICountriesRepository : IRepository<Country, int>
    {
        bool GetExists(string name);
    }
}
