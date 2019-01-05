using Core.Entities.Base;
using DAL.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories.Base.IRepository
{
    public interface ICitiesRepository : IRepository<City, int>
    {
        IEnumerable<City> GetByRegionId(int regionId);
        IEnumerable<City> GetAllWithCountriesAndRegions();
        IEnumerable<City> GetByCountryId(int countryId);
        bool GetExists(string name, int countryId);
    }
}
