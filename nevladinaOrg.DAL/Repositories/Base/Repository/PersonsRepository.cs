using System;
using System.Linq;
using System.Collections.Generic;

using DAL.Contexts;
using Core.Entities.Base;
using Core.Entities.Base.DTO;
using DAL.Repositories.Base.IRepository;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;

namespace DAL.Repositories.Base.Repository
{
    public class PersonsRepository : Repository<Person, int>, IPersonsRepository
    {
        public PersonsRepository(NevladinaOrgContext context) : base(context)
        {
        }
        public int NuberOfExists(string firstname,string lastname)
        {
            return Context.Persons.Where(x => !x.IsDeleted && x.FirstName.ToLower() == firstname.ToLower() && x.LastName.ToLower() == lastname.ToLower()).Count();
        }
        public IEnumerable<Person> GetByUserIds(List<int> UserIds)
        {
            return Context.Persons.Where(x => !x.IsDeleted && UserIds.Contains(x.Id));
        }
        public int Remove(int Id)
        {
            var person = Context.Persons.FirstOrDefault(x => x.Id == Id);
            Context.Persons.Remove(person);
            return person.Id;
        }

        public PersonDTO GetPersonDetails(int pPersonId)
        {
            var sql = "SELECT \"P\".\"Id\", \"P\".\"FirstName\", \"P\".\"LastName\", \"P\".\"ParentName\", \"P\".\"Gender\", \"P\".\"DateOfBirth\", EXTRACT(YEAR FROM age(\"P\".\"DateOfBirth\")) :: int AS \"Age\",\r\n\t\t\t\"U\".\"Username\", \"P\".\"BirthCountryId\" AS \"BirthCountryId\", \"P\".\"SocialSecurityNumber\", \"P\".\"NationalIDNumber\",  \"P\".\"CountryId\" AS \"CountryId\",\r\n\t\t\t\"P\".\"RegionId\" AS \"RegionId\", \"P\".\"CityId\" AS \"CityId\", \"P\".\"Place\", \"P\".\"CitizenshipId\", \"U\".\"Email\" AS \"UserEmail\", \"P\".\"Address\"\r\n\tFROM \"Persons\" AS \"P\"\r\n\tJOIN \"Users\" AS \"U\" ON \"P\".\"Id\" = \"U\".\"Id\"\r\n\tWHERE \"P\".\"Id\" = @pPersonId;";

            return DbConnection.Query<PersonDTO>(sql, new { pPersonId }).SingleOrDefault();
        }
        
        public IEnumerable<PersonDTO> GetPersons(IEnumerable<int> userIds)
        {
            var pUserIds = "0";
            if (userIds.Count() > 0)
            {
              pUserIds = String.Join(",", userIds);
            }
             
            var sql = "SELECT \"P\".\"Id\", \"P\".\"FirstName\", \"P\".\"LastName\", \"P\".\"Gender\", \"P\".\"DateOfBirth\", \"C\".\"Name\" AS \"Country\", \"CI\".\"Name\" AS \"City\", \r\n\t(SELECT \"PC\".\"Contact\"\r\n\tFROM \"PersonContacts\" AS \"PC\" WHERE \"PC\".\"PersonId\" = \"P\".\"Id\" AND \"PC\".\"ContactTypeId\" = 2\r\n\tLIMIT 1\r\n\t) AS \"Phone\",\r\n\tEXTRACT(YEAR FROM age(\"P\".\"DateOfBirth\")) :: int AS \"Age\"\r\n\tFROM \"Persons\" AS \"P\"\r\n\tJOIN \"Countries\" AS \"C\" ON \"C\".\"Id\" = \"P\".\"CountryId\"\r\n\tJOIN \"Cities\" AS \"CI\" ON \"CI\".\"Id\" = \"P\".\"CityId\"\r\n\tWHERE \"P\".\"Id\" IN (SELECT regexp_split_to_table :: int FROM regexp_split_to_table(@pUserIds, ','));";

            return DbConnection.Query<PersonDTO>(sql, new { pUserIds });
        }
    }
}
