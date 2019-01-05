using Core.Entities.Base;
using nevladinaOrg.Web.Helpers;
using System.ComponentModel.DataAnnotations;

namespace nevladinaOrg.Web.Areas.Administration.ViewModels
{
    public class MaritalStatusViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = nameof(Localizer.ErrorMessageNameReq)),
         StringLength(100, ErrorMessage = nameof(Localizer.ErrorMessageNameLen100))]
        public string Name { get; set; }



        public static implicit operator MaritalStatus(MaritalStatusViewModel model)
        {
            MaritalStatus maritalStatus = new MaritalStatus()
            {
                Id = model.Id,
                Name = model.Name
            };

            return maritalStatus;
        }

        public static implicit operator MaritalStatusViewModel(MaritalStatus model)
        {
            MaritalStatusViewModel maritalStatusViewModel = new MaritalStatusViewModel()
            {
                Id = model.Id,
                Name = model.Name
            };

            return maritalStatusViewModel;
        }
    }
}
