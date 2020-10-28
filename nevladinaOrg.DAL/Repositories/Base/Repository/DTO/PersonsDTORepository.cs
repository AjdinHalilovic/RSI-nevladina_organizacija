using Core.Entities.Base.DTO;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.Base.Repository.DTO
{
    public class PersonsDTORepository : Repository<PersonDTO, int>, IPersonsDTORepository
    {
        public PersonsDTORepository(NevladinaOrgContext context) : base(context)
        {
        }
        public IEnumerable<PersonDTO> GetByUserIds(List<int> userIds)
        {
            var persons = Context.Users.Include(x => x.Person).Include(x => x.Person.City).Where(x => !x.IsDeleted && userIds.Contains(x.Id))
                .Select(x => new PersonDTO
                {
                    Id=x.Id,
                    FirstName = x.Person.FirstName,
                    LastName = x.Person.LastName,
                    Gender = x.Person.Gender,
                    DateOfBirth = x.Person.DateOfBirth,
                    City = x.Person.City.Name,
                    Email=x.Email
                }).ToList();
            var contacts = Context.PersonContacts.Include(x => x.Person).Where(x => !x.IsDeleted && persons.Select(y => y.Id).Contains(x.PersonId)).ToList();
            foreach (var person in persons)
            {
                person.Phone = contacts.FirstOrDefault(y => y.ContactTypeId == 3 && person.Id == y.PersonId).Contact;
            }
            persons.ForEach(x => x.Phone = contacts.FirstOrDefault(y => y.ContactTypeId == 3 && x.Id==y.PersonId).Contact);
            return persons;
        }
    }
}
