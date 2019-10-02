using Core.Entities.Base;
using nevladinaOrg.Web.Constants;
using nevladinaOrg.Web.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace nevladinaOrg.Web.Areas.Administration.ViewModels
{
    public class RoleFunctionalityViewModel
    {
        public RoleViewModel Role { get; set; }
        [Required(ErrorMessage = nameof(Localizer.ErrorMessageFunctionalityReq))]
        public string FunctionalityIds { get; set; }
        public Enumerations.SaveAndOptions SaveAndOptions { get; set; }
    }
}
