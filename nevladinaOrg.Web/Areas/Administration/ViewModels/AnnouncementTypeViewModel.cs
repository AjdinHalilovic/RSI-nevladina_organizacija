using Core.Entities.Base;
using nevladinaOrg.Web.Constants;
using nevladinaOrg.Web.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nevladinaOrg.Web.Areas.Administration.ViewModels
{
    public class AnnouncementTypeViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = nameof(Localizer.ErrorMessageNameReq)),
         StringLength(100, ErrorMessage = nameof(Localizer.ErrorMessageNameLen100))]
        public string Name { get; set; }
        public Enumerations.SaveAndOptions SaveAndOptions { get; set; }

        public static implicit operator AnnouncementType(AnnouncementTypeViewModel model)
        {
            AnnouncementType announcementType = new AnnouncementType()
            {
                Id = model.Id,
                Name = model.Name
            };

            return announcementType;
        }

        public static implicit operator AnnouncementTypeViewModel(AnnouncementType model)
        {
            AnnouncementTypeViewModel announcementTypeVM = new AnnouncementTypeViewModel()
            {
                Id = model.Id,
                Name = model.Name,
                SaveAndOptions = Enumerations.SaveAndOptions.SaveAndClose
            };

            return announcementTypeVM;
        }
    }
}
