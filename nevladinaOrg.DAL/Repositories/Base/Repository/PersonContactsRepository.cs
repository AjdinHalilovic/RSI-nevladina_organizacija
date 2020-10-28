using Core.Entities.Base;
using Core.Entities.Base.DTO;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repositories.Base.Repository
{
    public class PersonContactsRepository:Repository<PersonContact,int>,IPersonContactsRepository
    {
        public PersonContactsRepository(NevladinaOrgContext context) : base(context) { }
        public IEnumerable<PersonContact> GetByPersonId(int personId)
        {
            return Context.PersonContacts.Where(x => !x.IsDeleted && x.PersonId == personId);
        }
        public PersonContact GetByPersonIdAndContactTypeId(int pPersonId, int pContactTypeId, bool? pPrimary = null)
        {
            var sql = "SELECT\t\"PC\".*\r\n\tFROM \"PersonContacts\" AS \"PC\"\r\n\tWHERE \"PC\".\"PersonId\" = @pPersonId \r\n\t\t\tAND \"PC\".\"IsDeleted\" = false \r\n\t\t\tAND \"PC\".\"ContactTypeId\" = @pContactTypeId \r\n\t\t\tAND (\"PC\".\"Primary\" = @pPrimary OR @pPrimary IS NULL)\r\n\tLIMIT 1;";

            var personContact = DbConnection.Query<PersonContact>(sql, new { pPersonId, pContactTypeId, pPrimary }).SingleOrDefault();

            if (personContact != null)
                Context.Attach(personContact);

            return personContact;
        }

        public IEnumerable<PersonContactDTO> GetPersonContactsByPersonId(int pPersonId, bool? pPrimary = null)
        {
            var sql = "SELECT\t\"PC\".\"Id\", \"PC\".\"Contact\", \"PC\".\"ContactTypeId\", \"PC\".\"Primary\"\r\n\tFROM \"PersonContacts\" AS \"PC\"\r\n\tWHERE \"PC\".\"PersonId\" = @pPersonId AND \"PC\".\"IsDeleted\" = false AND (\"PC\".\"Primary\" = @pPrimary OR @pPrimary IS NULL);";

            return DbConnection.Query<PersonContactDTO>(sql, new { pPersonId, pPrimary });
        }
    }
}
