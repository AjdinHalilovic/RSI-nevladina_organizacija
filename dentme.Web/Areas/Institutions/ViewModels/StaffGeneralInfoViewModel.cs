using nevladinaOrg.Web.Areas.Institutions.ViewModels;
using nevladinaOrg.Web.Areas.Organizations.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nevladinaOrg.Web.Areas.Institutions.ViewModels
{
    public class StaffGeneralInfoViewModel
    {
        public PersonViewModel Person { get; set; }
        public PersonContactViewModel PersonPhone { get; set; }
        public PersonContactViewModel PersonMobile { get; set; }
        public PersonContactViewModel PersonPrimaryEmail { get; set; }
        public PersonContactViewModel PersonSecondaryEmail { get; set; }
    }
}
