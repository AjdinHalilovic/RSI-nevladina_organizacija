using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories.Base.Repository
{
    public class OrganizationContactsRepository : Repository<OrganizationContact, int>, IOrganizationContactsRepository
    {
        public OrganizationContactsRepository(NevladinaOrgContext context) : base(context)
        {
        }
    }
}
