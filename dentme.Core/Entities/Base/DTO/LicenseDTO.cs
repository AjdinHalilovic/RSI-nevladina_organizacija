using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Base.DTO
{
    public class LicenseDTO
    {
        public int Id { get; set; }
        public int LicensePeriodId { get; set; }
        public string LicenseNumber { get; set; }
        public string LicenseType { get; set; }
        public string CreatedDate { get; set; }
        public string EndDate { get; set; }
        public bool Active { get; set; }

    }
}
