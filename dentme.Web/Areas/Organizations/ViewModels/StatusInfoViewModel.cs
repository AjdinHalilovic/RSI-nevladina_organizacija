using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nevladinaOrg.Web.Areas.Organizations.ViewModels
{
    public class StatusInfoViewModel
    {
        public PersonDetailsViewModel PersonDetail { get; set; }
        public bool Employer { get; set; }
        public bool Member { get; set; }
        public int InstitutionId { get; set; }
    }
}
