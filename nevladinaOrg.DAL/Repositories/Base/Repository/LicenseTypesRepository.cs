using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System.Linq;

namespace DAL.Repositories.Base.Repository
{
    public class LicenseTypesRepository:Repository<LicenseType,int>,ILicenseTypesRepository
    {
        public LicenseTypesRepository(NevladinaOrgContext context):base(context)
        {
        }
        public bool GetExists(string name)
        {
            return Context.LicenseTypes.Any(x => x.Name == name);
        }
    }
}
