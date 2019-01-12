using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repositories.Base.Repository
{
    public class EmployeeStatusesRepository : Repository<EmployeeStatus, int>, IEmployeeStatusesRepository
    {
        public EmployeeStatusesRepository(NevladinaOrgContext context) : base(context)
        { }

        public bool GetExists(string name)
        {
            return Context.EmployeeStatuses.Any(x => x.Name == name && x.IsDeleted == false);
        }
    }
}
