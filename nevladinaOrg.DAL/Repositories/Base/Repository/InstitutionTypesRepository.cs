using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repositories.Base.Repository
{
    public class InstitutionTypesRepository : Repository<InstitutionType, int>, IInstitutionTypesRepository
    {
        public InstitutionTypesRepository(NevladinaOrgContext context) : base(context)
        {
        }

        public bool GetExists(string name) => Context.InstitutionTypes.Any(x => x.Name == name && x.IsDeleted == false);
    }
}
