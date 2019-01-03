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
    public class RegionViewModel
    {
        [Key]
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = nameof(Localizer.ErrorMessageCountryReq))]
        public int CountryId { get; set; }

        [Required(ErrorMessage = nameof(Localizer.ErrorMessageNameReq)),
         StringLength(100, ErrorMessage = nameof(Localizer.ErrorMessageNameLen100))]
        public string Name { get; set; }


        public static implicit operator Region(RegionViewModel model)
        {
            return new Region(model.Name, model.CountryId);
        }

        public static implicit operator RegionViewModel(Region model)
        {
            RegionViewModel regionVM = new RegionViewModel()
            {
                Id = model.Id,
                CountryId = model.CountryId,
                Name = model.Name
            };

            return regionVM;
        }
    }
}
