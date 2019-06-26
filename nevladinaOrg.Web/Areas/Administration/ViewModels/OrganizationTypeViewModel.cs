using Core.Entities.Base;
using nevladinaOrg.Web.Helpers;
using System.ComponentModel.DataAnnotations;

namespace nevladinaOrg.Web.Areas.Administration.ViewModels
{
    public class OrganizationTypeViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = nameof(Localizer.ErrorMessageNameReq)),
         StringLength(100, ErrorMessage = nameof(Localizer.ErrorMessageNameLen100))]
        public string Name { get; set; }



        public static implicit operator OrganizationType(OrganizationTypeViewModel model)
        {
            OrganizationType organizationType = new OrganizationType()
            {
                Id = model.Id,
                Name = model.Name
            };

            return organizationType;
        }

        public static implicit operator OrganizationTypeViewModel(OrganizationType model)
        {
            OrganizationTypeViewModel organizationTypeViewModel = new OrganizationTypeViewModel()
            {
                Id = model.Id,
                Name = model.Name
            };

            return organizationTypeViewModel;
        }
    }
}
