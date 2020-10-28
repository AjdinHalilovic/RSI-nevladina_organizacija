using Core.Entities.Base;

namespace DAL.Repositories.Base.IRepository
{
    public interface IInstitutionTypesRepository : IRepository<InstitutionType, int>
    {
        bool GetExists(string name);
    }
}
