using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.Base.Repository
{
    public class OrganizationRepository : Repository<Organization, int>, IOrganizationRepository
    {

        public OrganizationRepository(NevladinaOrgContext context) : base(context)
        {
        }

        public IEnumerable<Organization> GetByOrganizationIds(int?[] organizationIds)
        {
            if ((organizationIds?.Length ?? 0) == 0)
                return null;

            return Context.Organizations.Where(x => organizationIds.Contains(x.Id));
        }

        public IEnumerable<Organization> GetByOrganizationIds(int[] organizationIds)
        {
            if (organizationIds.Length == 0)
                return null;

            return Context.Organizations.Where(x => organizationIds.Contains(x.Id));
        }

        public IEnumerable<Organization> GetByOrganizationIds(string organizationIds)
        {
            if (string.IsNullOrEmpty(organizationIds))
                return null;

            int[] _organizationIds = Array.ConvertAll(organizationIds.Split(','), Convert.ToInt32);

            return Context.Organizations.Where(x => _organizationIds.Contains(x.Id));
        }

        public bool GetExists(string name, int countryId, int cityId, int organizationTypeId)
        {
            return Context.Organizations.Any(x => x.Name == name && x.CityId == cityId && x.OrganizationTypeId == organizationTypeId && x.IsDeleted == false);
        }
    }
}
