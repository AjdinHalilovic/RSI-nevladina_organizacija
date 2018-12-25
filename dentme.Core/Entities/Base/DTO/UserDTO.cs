using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Base.DTO
{
    public class UserDTO
    {
        [Key]
        public int Id { get; set; }

        public Guid Token { get; set; }

        public int UserId { get; set; }
        public int InstitutionUserId { get; set; }
        public int InstitutionId { get; set; }
        public int MemberId { get; set; }
        public bool Employed { get; set; }

        public int? OrganizationInstitutionUserId { get; set; }
        public int? OrganizationId { get; set; }

        public int? PatientId => OrganizationInstitutionUserId;
        public int? StaffId => OrganizationInstitutionUserId;

        public DateTime? LastLogin { get; set; }
    }
}
