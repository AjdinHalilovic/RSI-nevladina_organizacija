using Core.Entities.Base.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nevladinaOrg.Web.ViewModels
{
    public class DashboardViewModel
    {
        public IEnumerable<EventDTO> Events { get; set; }
    }
}
