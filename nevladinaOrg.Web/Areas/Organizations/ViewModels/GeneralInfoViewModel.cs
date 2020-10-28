using nevladinaOrg.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nevladinaOrg.Web.Areas.Organizations.ViewModels
{
    public class GeneralInfoViewModel
    {
        public PersonViewModel Person { get; set; }
        public PersonContactViewModel PersonPhone { get; set; }
        public PersonContactViewModel PersonMobile { get; set; }
        public PersonContactViewModel PersonPrimaryEmail { get; set; }
        public PersonContactViewModel PersonSecondaryEmail { get; set; }
    }
}
