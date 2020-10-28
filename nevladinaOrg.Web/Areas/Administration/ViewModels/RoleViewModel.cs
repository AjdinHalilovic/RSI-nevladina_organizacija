using Core.Entities.Base;
using nevladinaOrg.Web.Constants;
using nevladinaOrg.Web.Helpers;
using System.ComponentModel.DataAnnotations;
namespace nevladinaOrg.Web.Areas.Administration.ViewModels
{
    public class RoleViewModel
    {
        [Key]
        public int Id { get; set; }
        public int? InstitutionId { get; set; }
        public int? OrganizationId { get; set; }
        [Required(ErrorMessage = nameof(Localizer.ErrorMessageNameReq)),
        StringLength(100, ErrorMessage = nameof(Localizer.ErrorMessageNameLen100))]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = nameof(Localizer.ErrorMessageActiveReq))]
        public bool Active { get; set; }
        public Enumerations.SaveAndOptions SaveAndOptions { get; set; }
        public static implicit operator Role(RoleViewModel model)
        {
            Role role = new Role()
            {
                Id = model.Id,
                Name = model.Name,
                //InstitutionId = model.InstitutionId,
                //OrganizationId = model.OrganizationId,
                Description = model.Description,
                Active = model.Active
            };
            return role;
        }
        public static implicit operator RoleViewModel(Role model)
        {
            RoleViewModel role = new RoleViewModel()
            {
                Id = model.Id,
                Name = model.Name,
                //InstitutionId = model.InstitutionId,
                //OrganizationId = model.OrganizationId,
                Description = model.Description,
                Active = model.Active
            };
            return role;
        }
    }
}