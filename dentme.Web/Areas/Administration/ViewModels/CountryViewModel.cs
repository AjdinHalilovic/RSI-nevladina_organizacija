using Core.Entities.Base;
using nevladinaOrg.Web.Constants;
using nevladinaOrg.Web.Helpers;
using System.ComponentModel.DataAnnotations;

namespace nevladinaOrg.Web.Areas.Administration.ViewModels
{
    public class CountryViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = nameof(Localizer.ErrorMessageNameReq)),
         StringLength(100, ErrorMessage = nameof(Localizer.ErrorMessageNameLen100))]
        public string Name { get; set; }


        public static implicit operator Country(CountryViewModel model)
        {
            Country country = new Country()
            {
                Id = model.Id,
                Name = model.Name
            };

            return country;
        }
        public static implicit operator CountryViewModel(Country model)
        {
            CountryViewModel country = new CountryViewModel()
            {
                Id = model.Id,
                Name = model.Name
            };

            return country;
        }
    }
}
