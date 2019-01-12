using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repositories.Base.Repository
{
    public class PersonDetailsRepository:Repository<PersonDetail,int>,IPersonDetailsRepository
    {
        public PersonDetailsRepository(NevladinaOrgContext context) : base(context) { }
        public int Remove(int userId)
        {
            var personDetail = Context.PersonDetails.FirstOrDefault(x => x.Id == userId);
            Context.PersonDetails.Remove(personDetail);
            return personDetail.Id;
        }
    }
}
