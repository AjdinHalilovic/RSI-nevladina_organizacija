using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Base.DTO
{
    public class OrganizationInstitutionUserDTO
    {
        public int Id { get; set; }
        public int InstitutionUserId { get; set; }
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
        public string User { get; set; }
        public bool Employed { get; set; }
        public bool Member { get; set; }
        public bool Active { get; set; }
    }
}
