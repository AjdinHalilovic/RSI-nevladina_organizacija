using Core.Entities.Base.DTO;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository.DTO;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DAL.Repositories.Base.Repository.DTO
{
    public class PersonUsersDTORepository : Repository<PersonUserDTO, int>, IPersonUsersDTORepository
    {
        public PersonUsersDTORepository(NevladinaOrgContext context) : base(context)
        {
        }

        public override PersonUserDTO GetById(int id)
        {
            return Context.Users.Include(x => x.Person).Where(x => x.Id == id && !x.IsDeleted).Select(x => new PersonUserDTO(x.Id, x.Person.FirstName, x.Person.LastName, x.Person.Gender, x.Person.DateOfBirth, x.Username, x.Email, x.CultureName, x.ChangedPassword)).FirstOrDefault();
        }
    }
}
