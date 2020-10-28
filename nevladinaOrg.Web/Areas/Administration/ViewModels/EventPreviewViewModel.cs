using Core.Entities.Base;
using Core.Entities.Base.DTO;
using System.Collections.Generic;

namespace nevladinaOrg.Web.Areas.Administration.ViewModels
{
    public class EventPreviewViewModel
    {
        public EventDTO EventDTO { get; set; }
        public List<EventItemDTO> EventItemsDTO { get; set; }
        public List<SponsorDTO>SponsorsDTO { get; set; }
        public Dictionary<string,string> SrcImages { get; set; }
        public List<EventDocument> Documents { get; set; }
        public bool UserAlreadyRegistred { get; set; }

    }
}
