using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nevladinaOrg.Web.ViewModels
{
    public class LoggedUserIdsViewModel
    {
        public int UserId { get; set; }

        public int InstitutionUserId { get; set; }
        public int? MemberId { get; set; }
        public int InstitutionId { get; set; }

        public int? OrganizationInstitutionUserId { get; set; }
        public int? OrganizationId { get; set; }

        public int? PatientId => OrganizationInstitutionUserId;
        public int? StaffId => OrganizationInstitutionUserId;
    }
}
