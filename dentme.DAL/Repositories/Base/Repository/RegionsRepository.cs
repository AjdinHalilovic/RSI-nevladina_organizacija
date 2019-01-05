using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repositories.Base.Repository
{
    public class RegionsRepository : Repository<Region, int>, IRegionsRepository
    {
        public RegionsRepository(NevladinaOrgContext context) : base(context)
        {
        }

       
        public IEnumerable<Region> GetByCountryId(int countryId)
        {
            return Context.Regions.Where(x => x.CountryId == countryId && x.IsDeleted == false);
        }

        public IEnumerable<Region> GetByNameAndCountryId(string name, int countryId) => Context.Regions.Where(x => x.Name == name && x.CountryId == countryId);
        public IEnumerable<Region> GetByCountryID(int CountryId) => Context.Regions.Where(x => x.CountryId == CountryId);
        public bool GetExists(string name, int countryId)
        {
            return Context.Regions.Where(x => x.IsDeleted == false).Any(x => x.Name == name && x.CountryId == countryId);
        }

        public bool GetExists(string name) => Context.Regions.Where(x => x.IsDeleted == false).Any(x => x.Name == name);
    }
}
