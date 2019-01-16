using Core.Entities.Base;
using nevladinaOrg.Web.Helpers;
using System.ComponentModel.DataAnnotations;

namespace nevladinaOrg.Web.Areas.Administration.ViewModels
{
    public class InstitutionTypeViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = nameof(Localizer.ErrorMessageNameReq)),
         StringLength(100, ErrorMessage = nameof(Localizer.ErrorMessageNameLen100))]
        public string Name { get; set; }



        public static implicit operator InstitutionType(InstitutionTypeViewModel model)
        {
            InstitutionType institutionType = new InstitutionType()
            {
                Id = model.Id,
                Name = model.Name
            };

            return institutionType;
        }
        public static implicit operator InstitutionTypeViewModel(InstitutionType model)
        {
            InstitutionTypeViewModel institutionType = new InstitutionTypeViewModel()
            {
                Id = model.Id,
                Name = model.Name
            };

            return institutionType;
        }
    }
}
