using Core.Entities.Base;

namespace DAL.Repositories.Base.IRepository
{
    public interface IContactTypesRepository : IRepository<ContactType, int>
    {
        bool GetExists(string name);
    }
}
