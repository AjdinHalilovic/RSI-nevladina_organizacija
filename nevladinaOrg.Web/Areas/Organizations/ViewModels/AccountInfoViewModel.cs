using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nevladinaOrg.Web.Areas.Organizations.ViewModels
{
    public class AccountInfoViewModel
    {
        public UserViewModel User { get; set; }
        public List<string> Roles { get; set; }
        public int InstitutionUserId { get; set; }
        public int? OrganizationInstitutionUserId { get; set; }

    }
}
