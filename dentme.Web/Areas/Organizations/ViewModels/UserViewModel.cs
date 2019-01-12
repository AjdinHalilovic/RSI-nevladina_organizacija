using Core.Entities.Base;
using nevladinaOrg.Web.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nevladinaOrg.Web.Areas.Organizations.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public Guid? Token { get; set; }
        [Required(ErrorMessage =nameof(Localizer.ErrorMessageUsernameReq))]
        public string Username { get; set; }
        [Required(ErrorMessage =nameof(Localizer.ErrorMessageEmailReq))]
        public string Email { get; set; }
        [Required(ErrorMessage =nameof(Localizer.ErrorMessagePasswordReq))]
        public string Password { get; set; }
        public string EditPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string CultureName { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool ChangedPassword { get; set; }

        public bool IsDeleted { get; set; }

        public static implicit operator User(UserViewModel model)
        {
            User user = new User()
            {
                Id = model.Id,
                ChangedPassword=model.ChangedPassword,
                CreatedDate=model.CreatedDate,
                CultureName=model.CultureName,
                Email=model.Email,
                PasswordHash=model.PasswordHash,
                PasswordSalt=model.PasswordSalt,
                Username=model.Username
            };

            return user;
        }

        public static implicit operator UserViewModel(User model)
        {
            UserViewModel user = new UserViewModel()
            {
                Id = model.Id,
                ChangedPassword = model.ChangedPassword,
                CreatedDate = model.CreatedDate,
                CultureName = model.CultureName,
                Email = model.Email,
                PasswordHash = model.PasswordHash,
                PasswordSalt = model.PasswordSalt,
                Username = model.Username
            };

            return user;
        }
    }
}
