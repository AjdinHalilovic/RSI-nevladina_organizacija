using Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nevladinaOrg.Web.Areas.Administration.ViewModels
{
    public class EventRegistrationViewModel
    {
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public EventRegistration registration { get; set; }
        public string DateOfRegistration { get { return registration.DateOfRegistration.ToString(); } }

    }
}
