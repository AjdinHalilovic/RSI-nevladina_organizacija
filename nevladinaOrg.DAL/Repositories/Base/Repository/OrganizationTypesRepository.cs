using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repositories.Base.Repository
{
    public class OrganizationTypesRepository : Repository<OrganizationType, int>, IOrganizationTypesRepository
    {

        public OrganizationTypesRepository(NevladinaOrgContext context) : base(context)
        {
        }

        public bool GetExists(string name) => Context.OrganizationTypes.Where(x => x.IsDeleted == false).Any(x => x.Name == name);
    }
}
