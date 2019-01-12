using Core.Entities.Base;
using Core.Entities.Base.DTO;
using System.Collections.Generic;

namespace DAL.Repositories.Base.IRepository
{
    public interface IPersonContactsRepository:IRepository<PersonContact, int>
    {
        IEnumerable<PersonContact> GetByPersonId(int personId);
        IEnumerable<PersonContactDTO> GetPersonContactsByPersonId(int pPersonId, bool? pPrimary = null);

        PersonContact GetByPersonIdAndContactTypeId(int pPersonId, int pContactTypeId, bool? pPrimary = null);
    }
}
