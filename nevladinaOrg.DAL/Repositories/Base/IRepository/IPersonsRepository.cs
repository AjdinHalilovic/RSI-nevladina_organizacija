using Core.Entities.Base;
using Core.Entities.Base.DTO;
using System.Collections.Generic;

namespace DAL.Repositories.Base.IRepository
{
    public interface IPersonsRepository : IRepository<Person, int>
    {
        IEnumerable<PersonDTO> GetPersons(IEnumerable<int> pUserIds);

        int NuberOfExists(string firstname, string lastname);
        IEnumerable<Person> GetByUserIds(List<int> UserIds);
        int Remove(int Id);
        PersonDTO GetPersonDetails(int pPersonId);
    }
}
