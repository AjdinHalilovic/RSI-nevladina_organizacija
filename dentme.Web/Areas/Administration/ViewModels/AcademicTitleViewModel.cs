using Core.Entities;
using Core.Entities.Base;
using nevladinaOrg.Web.Helpers;
using System.ComponentModel.DataAnnotations;

namespace nevladinaOrg.Web.Areas.Administration.ViewModels
{
    public class AcademicTitleViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = nameof(Localizer.ErrorMessageNameReq)),
         StringLength(100, ErrorMessage = nameof(Localizer.ErrorMessageNameLen100))]
        public string Name { get; set; }


        public static implicit operator AcademicTitle(AcademicTitleViewModel model)
        {
            AcademicTitle academicTitle = new AcademicTitle()
            {
                Id = model.Id,
                Name = model.Name
            };

            return academicTitle;
        }

        public static implicit operator AcademicTitleViewModel(AcademicTitle model)
        {
            AcademicTitleViewModel academicTitleVM = new AcademicTitleViewModel()
            {
                Id = model.Id,
                Name = model.Name
            };

            return academicTitleVM;
        }
    }
}
