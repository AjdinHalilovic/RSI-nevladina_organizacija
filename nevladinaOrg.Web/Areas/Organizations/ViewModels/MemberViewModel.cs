using Core.Entities.Base;
using System.Collections.Generic;

namespace nevladinaOrg.Web.Areas.Organizations.ViewModels
{
    public class MemberViewModel
    {
        public PersonViewModel Person { get; set; }
        public UserViewModel User { get; set; }
        public PersonDetailsViewModel PersonDetail { get; set; }
        public MemberLicenseViewModel License { get; set; }
        public LicensePeriodViewModel LicensePeriod { get; set; }
        public PersonPhoto PersonPhoto { get; set; }
        public string PersonPhotoBase64 { get; set; }
        public string LicenceTypePeriod { get; set; }
        public bool Employer { get; set; }
        public bool Member { get; set; }
        public List<string> Roles { get; set; }
        public PersonContactViewModel PersonPhone { get; set; }
        public PersonContactViewModel PersonMobile { get; set; }
        public PersonContactViewModel PersonPrimaryEmail { get; set; }
        public PersonContactViewModel PersonSecondaryEmail { get; set; }
        public string Note { get; set; }
        public int CurrentStep { get; set; }



        public GeneralInfoViewModel GeneralInfo { get; set; }
        public StatusInfoViewModel StatusInfo { get; set; }
        public AccountInfoViewModel AccountInfo { get; set; }
    }
}
