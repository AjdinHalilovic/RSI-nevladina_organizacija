using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base;
using DAL.Repositories.Base.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.Base.Repository
{
    public class CitiesRepository : Repository<City, int>, ICitiesRepository
    {
        public CitiesRepository(NevladinaOrgContext context) : base(context)
        {
        }
        public IEnumerable<City> GetAllWithCountriesAndRegions()
        {
            return Context.Cities.Include(x => x.Region).Include(x => x.Country).Where(x => x.IsDeleted == false);
        }

        public IEnumerable<City> GetByCountryId(int countryId)
        {
            return Context.Cities.Where(x => x.CountryId == countryId && x.IsDeleted == false);
        }

        public IEnumerable<City> GetByRegionId(int regionId)
        {
            return Context.Cities.Where(x => x.RegionId == regionId && x.IsDeleted == false);
        }

        public bool GetExists(string name, int countryId)
        {
            return Context.Cities.Any(x => x.Name == name && x.CountryId == countryId);
        }
    }
}
