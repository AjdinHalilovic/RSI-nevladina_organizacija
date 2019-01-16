using Core.Entities.Base;
using DAL.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories.Base.IRepository
{
    public interface IOrganizationRepository : IRepository<Organization, int>
    {
        bool GetExists(string name, int countryId, int cityId, int organizationTypeId);

        IEnumerable<Organization> GetByOrganizationIds(int?[] organizationIds);
        IEnumerable<Organization> GetByOrganizationIds(int[] organizationIds);

        IEnumerable<Organization> GetByOrganizationIds(string organizationIds);
    }
}
