using Core.Entities.Base;
using Core.Entities.Base.DTO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace nevladinaOrg.Web.ViewModels
{
    public class LoggedUserDataViewModel
    {
        [Required]
        public PersonUserDTO PersonUser { get; set; }

        [Required]
        public UserDTO User { get; set; }

        // ROLE && FUKCIONALNOSTI ???
        public IEnumerable<Functionality> Functionalities { get; set; }
        public IEnumerable<Role> Roles { get; set; }
    }
}