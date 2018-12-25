using Core.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace nevladinaOrg.Web.ViewModels.Access
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "UsernameIsRequired")]
        [DataType(DataType.Text)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "PasswordIsRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public int? InstitutionId { get; set; }
        public IEnumerable<Institution> Institutions { get; set; }

        public int? OrganizationId { get; set; }
        public IEnumerable<Organization> Organizations { get; set; }
    }
}
