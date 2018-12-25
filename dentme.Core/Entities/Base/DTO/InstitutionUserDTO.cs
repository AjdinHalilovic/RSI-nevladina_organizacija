using Core.Entities.Base.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Base.DTO
{
    public class InstitutionUserDTO
    {
        public int Id { get; set; }
        public int InstitutionId { get; set; }
        public int UserId { get; set; }
        public string User { get; set; }
        
        public bool Active { get; set; }
        public PersonUserDTO PersonUserDTO { get; set; }
    }
}
