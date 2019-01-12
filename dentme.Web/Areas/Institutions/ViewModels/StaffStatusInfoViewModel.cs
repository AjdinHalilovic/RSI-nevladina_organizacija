using nevladinaOrg.Web.Areas.Institutions.ViewModels;
using nevladinaOrg.Web.Areas.Organizations.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nevladinaOrg.Web.Areas.Institutions.ViewModels
{
    public class StaffStatusInfoViewModel
    {
        public PersonDetailsViewModel PersonDetail { get; set; }
        public int InstitutionId { get; set; }
    }
}
