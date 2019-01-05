using Core.Entities.Base;
using nevladinaOrg.Web.Helpers;
using System.ComponentModel.DataAnnotations;

namespace nevladinaOrg.Web.Areas.Administration.ViewModels
{
    public class AcademicDegreeViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = nameof(Localizer.ErrorMessageNameReq)),
         StringLength(100, ErrorMessage = nameof(Localizer.ErrorMessageNameLen100))]
        public string Name { get; set; }



        public static implicit operator AcademicDegree(AcademicDegreeViewModel model)
        {
            AcademicDegree academicDegree = new AcademicDegree()
            {
                Id = model.Id,
                Name = model.Name
            };

            return academicDegree;
        }

        public static implicit operator AcademicDegreeViewModel(AcademicDegree model)
        {
            AcademicDegreeViewModel academicDegreeViewModel = new AcademicDegreeViewModel()
            {
                Id = model.Id,
                Name = model.Name
            };

            return academicDegreeViewModel;
        }
    }
}
