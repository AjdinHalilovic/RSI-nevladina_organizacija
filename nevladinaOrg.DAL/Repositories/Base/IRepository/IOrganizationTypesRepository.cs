using Core.Entities.Base;

namespace DAL.Repositories.Base.IRepository
{
    public interface IOrganizationTypesRepository : IRepository<OrganizationType, int>
    {
        bool GetExists(string name);
    }
}
