using Core.Entities.Base;

namespace DAL.Repositories.Base.IRepository
{
    public interface ILicenseTypesRepository:IRepository<LicenseType,int>
    {
        bool GetExists(string name);
    }
}
