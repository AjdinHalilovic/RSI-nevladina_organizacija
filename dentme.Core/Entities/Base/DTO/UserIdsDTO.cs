using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Base.DTO
{
    public class UserIdsDTO
    {
        public int UserId { get; set; }

        public int InstitutionUserId { get; set; }
        public int? MemberId => InstitutionUserId;

        public int? OrganizationInstitutionUserId { get; set; }

        public int? PatientId => OrganizationInstitutionUserId;
        public int? StaffId => OrganizationInstitutionUserId;
    }
}
