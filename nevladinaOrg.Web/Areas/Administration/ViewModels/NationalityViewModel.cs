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
    public class NationalityViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = nameof(Localizer.ErrorMessageNameReq)),
         StringLength(100, ErrorMessage = nameof(Localizer.ErrorMessageNameLen100))]
        public string Name { get; set; }


        public static implicit operator Nationality(NationalityViewModel model)
        {
            Nationality nationality = new Nationality()
            {
                Id = model.Id,
                Name = model.Name
            };

            return nationality;
        }
        public static implicit operator NationalityViewModel(Nationality model)
        {
            NationalityViewModel nationality = new NationalityViewModel()
            {
                Id = model.Id,
                Name = model.Name
            };

            return nationality;
        }
    }
}
