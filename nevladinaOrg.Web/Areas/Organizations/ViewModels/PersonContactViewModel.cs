using Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nevladinaOrg.Web.Areas.Organizations.ViewModels
{
    public class PersonContactViewModel
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int ContactTypeId { get; set; }
        public string Contact { get; set; }
        public bool IsDeleted { get; set; }

        public static implicit operator PersonContact(PersonContactViewModel model)
        {
            PersonContact contact = new PersonContact()
            {
                Id = model.Id,
                Contact = model.Contact,
                ContactTypeId = model.ContactTypeId,
                PersonId = model.PersonId
            };

            return contact;
        }

        public static implicit operator PersonContactViewModel(PersonContact model)
        {
            PersonContactViewModel contact = new PersonContactViewModel()
            {
                Id = model.Id,
                Contact = model.Contact,
                ContactTypeId = model.ContactTypeId,
                PersonId = model.PersonId
            };

            return contact;
        }
    }
}
