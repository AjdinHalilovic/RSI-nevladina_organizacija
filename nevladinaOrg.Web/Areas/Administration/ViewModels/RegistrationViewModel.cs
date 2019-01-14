using Core.Entities.Base;
using nevladinaOrg.Web.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nevladinaOrg.Web.Areas.Administration.ViewModels
{
    public class RegistrationViewModel
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public IEnumerable<EventRegistration> eventRegistrations { get; set; }
        public List<EventRegistrationViewModel> registrations { get; set; }
        public Enumerations.SaveAndOptions SaveAndOptions { get; set; }

    }
}
