using Core.Entities.Base;
using System.Collections.Generic;

namespace DAL.Repositories.Base.IRepository
{
    public interface IRegionsRepository : IRepository<Region, int>
    {
        IEnumerable<Region> GetByNameAndCountryId(string name, int countryId);
        IEnumerable<Region> GetByCountryId(int countryId);
        bool GetExists(string name, int countryId);
    }
}
