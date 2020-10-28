using Core.Entities.Base;
using nevladinaOrg.Web.Constants;
using nevladinaOrg.Web.Helpers;
using System.ComponentModel.DataAnnotations;

namespace nevladinaOrg.Web.Areas.Administration.ViewModels
{
    public class CityViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = nameof(Localizer.ErrorMessageCountryReq)), 
         Range(1, int.MaxValue, ErrorMessage = nameof(Localizer.ErrorMessageCountryReq))]
        public int CountryId { get; set; }

        public int? RegionId { get; set; }

        [Required(ErrorMessage = nameof(Localizer.ErrorMessageNameReq)),
         StringLength(100, ErrorMessage = nameof(Localizer.ErrorMessageNameLen100))]
        public string Name { get; set; }


        public static implicit operator City(CityViewModel model)
        {
            City city = new City()
            {
                Id = model.Id,
                CountryId = model.CountryId,
                RegionId = model.RegionId,
                Name = model.Name
            };

            return city;
        }

        public static implicit operator CityViewModel(City model)
        {
            CityViewModel city = new CityViewModel()
            {
                Id = model.Id,
                CountryId = model.CountryId,
                RegionId = model.RegionId,
                Name = model.Name
            };

            return city;
        }
    }
}
