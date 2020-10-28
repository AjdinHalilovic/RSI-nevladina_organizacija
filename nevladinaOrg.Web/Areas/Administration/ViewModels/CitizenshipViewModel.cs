using Core.Entities.Base;
using nevladinaOrg.Web.Constants;
using nevladinaOrg.Web.Helpers;
using System.ComponentModel.DataAnnotations;


namespace nevladinaOrg.Web.Areas.Administration.ViewModels
{
    public class CitizenshipViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = nameof(Localizer.ErrorMessageNameReq)),
         StringLength(100, ErrorMessage = nameof(Localizer.ErrorMessageNameLen100))]
        public string Name { get; set; }



        public static implicit operator Citizenship(CitizenshipViewModel model)
        {
            Citizenship citizenship = new Citizenship()
            {
                Id = model.Id,
                Name = model.Name
            };

            return citizenship;
        }
        public static implicit operator CitizenshipViewModel(Citizenship model)
        {
            CitizenshipViewModel citizenship = new CitizenshipViewModel()
            {
                Id = model.Id,
                Name = model.Name
            };

            return citizenship;
        }
    }
}
