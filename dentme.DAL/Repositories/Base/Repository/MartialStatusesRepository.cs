using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repositories.Base.Repository
{
    public class MartialStatusesRepository : Repository<MaritalStatus, int>, IMartialStatusesRepository
    {
        public MartialStatusesRepository(NevladinaOrgContext context) : base(context)
        {
        }

        public bool GetExists(string name)
        {
            return Context.MaritalStatuses.Any(x => x.Name == name && !x.IsDeleted);
        }
    }
}
