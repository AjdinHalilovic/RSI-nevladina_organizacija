using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nevladinaOrg.Web.Areas.Organizations.ViewModels
{
    public class LicenseAddViewModel
    {
        public MemberLicenseViewModel License { get; set; }
        public LicensePeriodViewModel LicensePeriod { get; set; }
        public string LicenceTypePeriod { get; set; }
    }
}
