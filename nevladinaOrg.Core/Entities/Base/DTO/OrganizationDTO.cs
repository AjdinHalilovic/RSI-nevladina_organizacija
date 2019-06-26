using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities.Base.DTO
{
    public class OrganizationDTO
    {
        public int Id { get; set; }
        public string Parent { get; set; }
        public string OrganizationType { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string AdditionalInformation { get; set; }
        public bool Active { get; set; }
    }
}
