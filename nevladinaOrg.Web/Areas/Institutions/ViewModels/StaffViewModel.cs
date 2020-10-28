using Core.Entities.Base;
using nevladinaOrg.Web.Areas.Institutions.ViewModels;
using nevladinaOrg.Web.Areas.Organizations.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nevladinaOrg.Web.Areas.Institutions.ViewModels
{
    public class StaffViewModel
    {
        public StaffGeneralInfoViewModel GeneralInfo { get; set; }
        public StaffStatusInfoViewModel StatusInfo { get; set; }
        public StaffAccountInfoViewModel AccountInfo { get; set; }
        public PersonPhoto PersonPhoto { get; set; }
        public string PersonPhotoBase64 { get; set; }

        public MemberLicenseViewModel License { get; set; }
        public bool Member { get; set; }
        public int CurrentStep { get; set; }

    }
}
