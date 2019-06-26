using Core.Entities.Base;
using DAL;
using DAL.Repositories.Base.IRepository;
using nevladinaOrg.Web.Helpers;
using nevladinaOrg.Web.Helpers.Breadcrumb;
using nevladinaOrg.Web.Helpers.Culture;
using nevladinaOrg.Web.ViewModels;
using nevladinaOrg.Web.ViewModels.Access;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace nevladinaOrg.Web.Controllers
{
    public class AccessController : BaseController
    {
        private readonly IDataUnitOfWork _dataUnitOfWork;

        private readonly ICulture _culture;

        public AccessController(IDataUnitOfWork dataUnitOfWork,
                                ICulture culture,
                                IBreadcrumb breadcrumb,
                                IStringLocalizerFactory stringLocalizerFactory) : base(breadcrumb, stringLocalizerFactory)
        {
            _dataUnitOfWork = dataUnitOfWork;
            _culture = culture;
        }

        #region Login

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _dataUnitOfWork.BaseUow.UsersRepository.GetByUsername(model.Username);
            if (user == null || !Cryptography.Hash.Validate(model.Password, user.PasswordSalt, user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, Localizer.IncorrectUsernameOrPassword);
                return View(model);
            }
            var baseUser = _dataUnitOfWork.BaseUow.UsersRepository.GetById(user.Id);
            

            var userAccounts = _dataUnitOfWork.BaseUow.UsersDTORepository.GetByUserId(user.Id).ToList();
            var userAccount = userAccounts.FirstOrDefault();
            var personUser = _dataUnitOfWork.BaseUow.PersonUsersDTORepository.GetById(userAccount.UserId);

            /* POTREBNO DOVRŠITI - RAZDVOJI U DVIJE AKCIJE */

            //if (userAccounts.Count > 1)
            //{
            //    int[] institutionIds = userAccounts.Where(x => !x.OrganizationId.HasValue).GroupBy(x => x.InstitutionId).Select(x => x.Key).ToArray();
            //    int?[] organizationIds = userAccounts.Where(x => x.OrganizationId.HasValue).GroupBy(x => x.OrganizationId).Select(x => x.Key).ToArray();

            //    if (institutionIds.Length + (organizationIds?.Length ?? 0) != 1)
            //    {
            //        model.Institutions = institutionIds.Length != 0 ? _dataUnitOfWork.AuthUow.InstitutionsAuthRepository.GetByInstitutionIds(institutionIds) : null;
            //        model.Organizations = (organizationIds?.Length ?? 0) != 0 ? _dataUnitOfWork.AuthUow.OrganizationsAuthRepository.GetByOrganizationIds(organizationIds) : null;
            //    }
            //}

            IEnumerable<Functionality> functionalities = null;
            if (userAccount.OrganizationInstitutionUserId.HasValue)
                functionalities = _dataUnitOfWork.BaseUow.FunctionalitiesRepository.GetByOrganizationInstitutionUserId(userAccount.OrganizationInstitutionUserId.Value);
            else
                functionalities = _dataUnitOfWork.BaseUow.FunctionalitiesRepository.GetByInstitutionUserId(userAccount.InstitutionUserId);


            IEnumerable<Role> roles = null;
            if (userAccount.OrganizationInstitutionUserId.HasValue)
                roles = _dataUnitOfWork.BaseUow.RolesRepository.GetByOrganizationInstitutionUserId(userAccount.OrganizationInstitutionUserId.Value);
            else
                roles = _dataUnitOfWork.BaseUow.RolesRepository.GetByInstitutionUserId(userAccount.InstitutionUserId);

            var loggedUserViewModel = new LoggedUserDataViewModel
            {
                User = userAccount,
                PersonUser = personUser,
                Functionalities = functionalities,
                Roles=roles
            };

            if (loggedUserViewModel.User.OrganizationInstitutionUserId.HasValue)
               _dataUnitOfWork.BaseUow.OrganizationInstitutionUsersRepository.SetLastLogin(loggedUserViewModel.User.OrganizationInstitutionUserId.Value, DateTime.Now);
            else
                _dataUnitOfWork.BaseUow.InstitutionUsersRepository.SetLastLogin(loggedUserViewModel.User.InstitutionUserId, DateTime.Now);

            HttpContext.SetLoggedUser(loggedUserViewModel, model.RememberMe);

            _culture.Set(user.CultureName);

            //if (!string.IsNullOrWhiteSpace(model.RedirectTo))
                //return Redirect(model.RedirectTo);

            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Logout

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SetLoggedUser(null);

            return RedirectToAction(nameof(Login));
        }

        #endregion
    }
}