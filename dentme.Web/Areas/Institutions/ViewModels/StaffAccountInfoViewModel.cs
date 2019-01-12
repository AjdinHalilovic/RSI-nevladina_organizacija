using nevladinaOrg.Web.Areas.Institutions.ViewModels;
using nevladinaOrg.Web.Areas.Organizations.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nevladinaOrg.Web.Areas.Institutions.ViewModels
{
    public class StaffAccountInfoViewModel
    {
        public UserViewModel User { get; set; }
        public List<string> Roles { get; set; }
        public int InstitutionUserId { get; set; }
    }
}
