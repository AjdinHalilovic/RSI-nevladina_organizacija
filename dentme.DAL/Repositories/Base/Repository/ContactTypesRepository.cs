using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System.Linq;

namespace DAL.Repositories.Base.Repository
{
    public class ContactTypesRepository : Repository<ContactType, int>, IContactTypesRepository
    {
        public ContactTypesRepository(NevladinaOrgContext context) : base(context)
        {
        }

        public bool GetExists(string name)
        {
            return Context.ContactTypes.Any(x => x.Name == name && !x.IsDeleted);
        }
    }
}
