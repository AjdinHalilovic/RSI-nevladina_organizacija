using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Core.Constants;
using Core.Entities.Base;
using Core.Entities.Base.DTO;
using DAL;
using DAL.Repositories.Base.IRepository;
using DAL.Repositories.Base.IRepository.DTO;
using nevladinaOrg.Web.Areas.Institutions.ViewModels;
using nevladinaOrg.Web.Areas.Organizations.ViewModels;
using nevladinaOrg.Web.Constants;
using nevladinaOrg.Web.Controllers;
using nevladinaOrg.Web.Helpers;
using nevladinaOrg.Web.Helpers.Breadcrumb;
using nevladinaOrg.Web.Helpers.ImageHelper;
using nevladinaOrg.Web.Helpers.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Authorization = nevladinaOrg.Web.Helpers.Authorization;

namespace nevladinaOrg.Web.Areas.Institutions.Controllers
{
    [Area(MagicStrings.AreaNames.Institutions)]
    [WebRoles(Enumerations.WebRoles.InstitutionAdministrator)]
    public class StaffController : BaseController
    {
        #region Properties
        IDataUnitOfWork _dataUnitOfWork;

        private Localizer _localizer;
        private ILogger _logger;
        private IImageHelper _imageHelper;
        #endregion

        public StaffController(IDataUnitOfWork dataUnitOfWork,
            Localizer localizer, ILogger logger, IImageHelper imageHelper,
            IBreadcrumb breadcrumb, IStringLocalizerFactory stringLocalizerFactory)
                    : base(breadcrumb, stringLocalizerFactory)
        {
            _dataUnitOfWork = dataUnitOfWork;

            _localizer = localizer;
            _logger = logger;
            _imageHelper = imageHelper;
        }


        #region Index

        public IActionResult Index()
        {
            return View();
        }
        public JsonResult JsonIndexLicense(int MemberId)
        {
            var licenseDTO = _dataUnitOfWork.BaseUow.LicenseDTORepository.GetByMemberId(MemberId).ToList();
            var licenseDTOJson = JsonConvert.SerializeObject(licenseDTO);
            return new JsonResult(licenseDTOJson);
        }
        public JsonResult JsonIndex()
        {
            //IEnumerable<OrganizationInstitutionUserDTO> organizationInstitutionUsersDTO = null;
            //List<PersonDTO> personsDTO = null;
            //if (LoggedUserIds.OrganizationId.HasValue)
            //{
            //    organizationInstitutionUsersDTO = _dataUnitOfWork.BaseUow.OrganizationInstitutionUsersRepositoryDTORepository.GetAllByOrganizationId(LoggedUserIds.OrganizationId.Value).ToList();   //LoggedUser.organizationID 
            //}
            //if (organizationInstitutionUsersDTO != null)
            //{
            //    //var users = _dataUnitOfWork.BaseUow.PersonsRepository.GetByUserIds(organizationInstitutionUsersDTO.Select(x => x.UserId).ToList());
            //    //foreach (var institutionUser in organizationInstitutionUsersDTO)
            //    //{
            //    //    institutionUser.User = users.FirstOrDefault(x => x.Id == institutionUser.UserId).FirstName + " " + users.FirstOrDefault(x => x.Id == institutionUser.UserId).LastName;
            //    //}
            //    //OrganizationInstitutionUserId,InstiuttionUserId,UserId
            //    List<Tuple<int, int, int>> organizationInstitutionUsersIds = organizationInstitutionUsersDTO.Select(x => new Tuple<int, int, int>(x.Id, x.InstitutionUserId, x.UserId)).ToList();
            //    personsDTO = _personsDTORepository.GetByUserIds(organizationInstitutionUsersIds.Select(x => x.Item3).ToList()).ToList();
            //    foreach (var person in personsDTO)
            //    {
            //        person.OrganizationInstitutionUserId = organizationInstitutionUsersIds.FirstOrDefault(x => x.Item3 == person.Id).Item1;
            //        person.InstitutionUserId  = organizationInstitutionUsersIds.FirstOrDefault(x => x.Item3 == person.Id).Item2;
            //    }

            //}
            //var personsDTOJson = JsonConvert.SerializeObject(personsDTO);
            //return new JsonResult(personsDTOJson);

            var institutionUsersDTO = _dataUnitOfWork.BaseUow.InstitutionUsersDTORepository.GetByInstitutionId(LoggedUserIds.InstitutionId).ToList();


            List<Tuple<int, int, int>> institutionUsersIds = institutionUsersDTO.Select(x => new Tuple<int, int, int>(x.Id, x.InstitutionId, x.UserId)).ToList();
            var personsDTO = _dataUnitOfWork.BaseUow.PersonsDTORepository.GetByUserIds(institutionUsersIds.Select(x => x.Item3).ToList()).ToList();
            foreach (var person in personsDTO)
            {
                person.InstitutionUserId = institutionUsersIds.FirstOrDefault(x => x.Item3 == person.Id).Item1;
                person.InstitutionId = institutionUsersIds.FirstOrDefault(x => x.Item3 == person.Id).Item2;
            }

            var institutionUsersDTOJson = JsonConvert.SerializeObject(personsDTO);
            return new JsonResult(institutionUsersDTOJson);
        }

        public JsonResult JsonIndexEvents(int PersonId)
        {
            IEnumerable<EventDTO> events = _dataUnitOfWork.BaseUow.EventsDTORepository.GetByUserId(PersonId);

            var eventsJson = JsonConvert.SerializeObject(events);

            return new JsonResult(eventsJson);
        }
        #endregion

        #region Add

        public IActionResult Add()
        {
            var model = new StaffViewModel()
            {
                GeneralInfo = new StaffGeneralInfoViewModel
                {
                    Person = new PersonViewModel(),
                    PersonMobile = new PersonContact() { ContactTypeId = (int)Enumerations.ContactTypes.MobileNumber },
                    PersonPhone = new PersonContact() { ContactTypeId = (int)Enumerations.ContactTypes.PhoneNumber },
                    PersonPrimaryEmail = new PersonContact() { ContactTypeId = (int)Enumerations.ContactTypes.PrimaryEmail },
                    PersonSecondaryEmail = new PersonContact() { ContactTypeId = (int)Enumerations.ContactTypes.SecondaryEmail }
                },
                StatusInfo = new StaffStatusInfoViewModel
                {
                    PersonDetail = new PersonDetailsViewModel()
                },
                AccountInfo = new StaffAccountInfoViewModel
                {
                    Roles = new List<string>(),
                    User = new UserViewModel()
                }
            };
            return PartialView(MagicStrings.ViewNames._Add, model);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Add(StaffViewModel model)
        {
            if (model.GeneralInfo.PersonPhone.Contact == null)
            {
                ModelState.AddModelError(nameof(Localizer.ErrorMessageContactPhoneReq), Localizer.ErrorMessageContactPhoneReq);
            }
            if (model.GeneralInfo.PersonPrimaryEmail.Contact == null)
            {
                ModelState.AddModelError(nameof(Localizer.ErrorMessagePrimaryEmailReq), Localizer.ErrorMessagePrimaryEmailReq);
            }
            var pattern = new Regex("(?=.*[0-9])(?=.*[a-zA-Z]).");
            if (!pattern.IsMatch(model.AccountInfo.User.Password))
            {
                ModelState.AddModelError(nameof(Localizer.ErrorMessagePasswordReqContainLettersAndNumbers), Localizer.ErrorMessagePasswordReqContainLettersAndNumbers);
            }
            if (model.AccountInfo.User.Password.Count() < 6)
            {
                ModelState.AddModelError(nameof(Localizer.ErrorMessagePasswordLen6), Localizer.ErrorMessagePasswordLen6);
            }

            if (model.CurrentStep == 3 && ModelState.IsValid)
            {
                model.CurrentStep = 4;
                return PartialView(MagicStrings.ViewNames._Add, model);
            }
            if (ModelState.IsValid)
            {

                using (IDbContextTransaction contextTransaction = _dataUnitOfWork.BaseUow.InstitutionUsersRepository.BeginTransaction())
                {
                    model.GeneralInfo.Person.CitizenshipId = model.GeneralInfo.Person.CitizenshipId == 0 ? null : model.GeneralInfo.Person.CitizenshipId;
                    model.GeneralInfo.Person.BirthCityId = model.GeneralInfo.Person.BirthCityId == 0 ? null : model.GeneralInfo.Person.BirthCityId;
                    model.GeneralInfo.Person.RegionId = model.GeneralInfo.Person.RegionId == 0 ? null : model.GeneralInfo.Person.RegionId;
                    model.GeneralInfo.Person.BirthCountryId = model.GeneralInfo.Person.BirthCountryId == 0 ? null : model.GeneralInfo.Person.BirthCountryId;
                    model.GeneralInfo.Person.ResidenceId = model.GeneralInfo.Person.ResidenceId == 0 ? null : model.GeneralInfo.Person.ResidenceId;
                    model.StatusInfo.PersonDetail.MartialStatusId = model.StatusInfo.PersonDetail.MartialStatusId == 0 ? null : model.StatusInfo.PersonDetail.MartialStatusId;
                    model.StatusInfo.PersonDetail.AcademicTitleId = model.StatusInfo.PersonDetail.AcademicTitleId == 0 ? null : model.StatusInfo.PersonDetail.AcademicTitleId;
                    model.StatusInfo.PersonDetail.EmploymentStatusId = model.StatusInfo.PersonDetail.EmploymentStatusId == 0 ? null : model.StatusInfo.PersonDetail.EmploymentStatusId;

                    var user = new User();
                    user = model.AccountInfo.User;

                    var person = new Person();
                    person = model.GeneralInfo.Person;

                    var personDetails = new PersonDetail();
                    personDetails = model.StatusInfo.PersonDetail;


                    try
                    {
                        //
                        _dataUnitOfWork.BaseUow.PersonsRepository.Add(person);
                        _dataUnitOfWork.BaseUow.PersonsRepository.SaveChanges();

                        personDetails.Id = person.Id;
                        _dataUnitOfWork.BaseUow.PersonDetailsRepository.Add(personDetails);

                        user.Id = person.Id;
                        user.CultureName = LoggedUserData.PersonUser.CultureName;
                        user.CreatedDate = DateTime.Now;
                        user.ChangedPassword = true;
                        user.PasswordSalt = Cryptography.Salt.Create();
                        user.PasswordHash = Cryptography.Hash.Create(model.AccountInfo.User.Password, user.PasswordSalt);

                        _dataUnitOfWork.BaseUow.UsersRepository.Add(user);
                        _dataUnitOfWork.BaseUow.UsersRepository.SaveChanges();

                        model.GeneralInfo.PersonPrimaryEmail.PersonId = person.Id;
                        model.GeneralInfo.PersonPhone.PersonId = person.Id;
                        _dataUnitOfWork.BaseUow.PersonContactsRepository.Add(model.GeneralInfo.PersonPrimaryEmail);
                        _dataUnitOfWork.BaseUow.PersonContactsRepository.Add(model.GeneralInfo.PersonPhone);

                        if (model.GeneralInfo.PersonSecondaryEmail.Contact != null)
                        {
                            model.GeneralInfo.PersonSecondaryEmail.PersonId = person.Id;
                            _dataUnitOfWork.BaseUow.PersonContactsRepository.Add(model.GeneralInfo.PersonSecondaryEmail);
                        }
                        if (model.GeneralInfo.PersonMobile.Contact != null)
                        {
                            model.GeneralInfo.PersonMobile.PersonId = person.Id;
                            _dataUnitOfWork.BaseUow.PersonContactsRepository.Add(model.GeneralInfo.PersonMobile);
                        }
                        _dataUnitOfWork.BaseUow.PersonContactsRepository.SaveChanges();


                        var institutionUser = new InstitutionUser()
                        {
                            Active = true,
                            InstitutionId = LoggedUserIds.InstitutionId,
                            LastLogin = DateTime.Now,
                            UserId = user.Id
                        };
                        _dataUnitOfWork.BaseUow.InstitutionUsersRepository.Add(institutionUser);
                        _dataUnitOfWork.BaseUow.InstitutionUsersRepository.SaveChanges();

                        //var organizationInstitutionUser = new OrganizationInstitutionUser
                        //{
                        //    Active = true,
                        //    InstitutionUserId = institutionUser.Id,
                        //    OrganizationId = LoggedUserIds.OrganizationId.GetValueOrDefault()
                        //};
                        //_dataUnitOfWork.BaseUow.OrganizationInstitutionUsersRepository.Add(organizationInstitutionUser);
                        //_dataUnitOfWork.BaseUow.OrganizationInstitutionUsersRepository.SaveChanges();


                        var addedUserRoles = new List<int>();
                        foreach (var role in model.AccountInfo.Roles)
                        {
                            var userRole = new UserRole()
                            {
                                InstitutionUserId = institutionUser.Id,
                                OrganizationInstitutionUserId = null,
                                RoleId = int.Parse(role),
                            };
                            _dataUnitOfWork.BaseUow.UserRolesRepository.Add(userRole);
                            _dataUnitOfWork.BaseUow.UserRolesRepository.SaveChanges();
                            addedUserRoles.Add(userRole.Id);
                        }


                        contextTransaction.Commit();

                        #region Logiranje
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.Persons, person.Id, GetControllerName(), GetActionName(), null);
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.Users, user.Id, GetControllerName(), GetActionName(), null);
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.PersonDetails, personDetails.Id, GetControllerName(), GetActionName(), null);
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.PersonContacts, model.GeneralInfo.PersonPrimaryEmail.Id, GetControllerName(), GetActionName(), null);
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.PersonContacts, model.GeneralInfo.PersonPhone.Id, GetControllerName(), GetActionName(), null);
                        if (model.GeneralInfo.PersonMobile.Contact != null)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.PersonContacts, model.GeneralInfo.PersonMobile.Id, GetControllerName(), GetActionName(), null);
                        }
                        if (model.GeneralInfo.PersonSecondaryEmail.Contact != null)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.PersonContacts, model.GeneralInfo.PersonMobile.Id, GetControllerName(), GetActionName(), null);
                        }
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.InstitutionUsers, institutionUser.Id, GetControllerName(), GetActionName(), null);
                        foreach (var role in addedUserRoles)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.UserRoles, role, GetControllerName(), GetActionName(), null);
                        }
                        #endregion
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        contextTransaction.Rollback();

                        ModelState.AddModelError(nameof(Localizer.Error), Localizer.Error);
                        _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Add, Tables.Base.Users, user.Id, GetControllerName(), GetActionName(), ex);
                    }
                }
            }
            return PartialView(MagicStrings.ViewNames._Add, model);
        }

        #endregion

        #region Edit

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditGeneralInfo(StaffGeneralInfoViewModel model)
        {
            if (model.PersonPhone.Contact == null)
            {
                ModelState.AddModelError(nameof(Localizer.ErrorMessageContactPhoneReq), Localizer.ErrorMessageContactPhoneReq);
            }
            if (model.PersonPrimaryEmail.Contact == null)
            {
                ModelState.AddModelError(nameof(Localizer.ErrorMessagePrimaryEmailReq), Localizer.ErrorMessagePrimaryEmailReq);
            }
            if (ModelState.IsValid)
            {
                using (IDbContextTransaction contextTransaction = _dataUnitOfWork.BaseUow.UsersRepository.BeginTransaction())
                {
                    model.Person.CitizenshipId = model.Person.CitizenshipId == 0 ? null : model.Person.CitizenshipId;
                    model.Person.BirthCityId = model.Person.BirthCityId == 0 ? null : model.Person.BirthCityId;
                    model.Person.RegionId = model.Person.RegionId == 0 ? null : model.Person.RegionId;
                    model.Person.BirthCountryId = model.Person.BirthCountryId == 0 ? null : model.Person.BirthCountryId;
                    model.Person.ResidenceId = model.Person.ResidenceId == 0 ? null : model.Person.ResidenceId;

                    PersonContact secondaryEmail = null;
                    PersonContact mobile = null;

                    try
                    {
                        _dataUnitOfWork.BaseUow.PersonsRepository.Update(model.Person);
                        _dataUnitOfWork.BaseUow.PersonContactsRepository.Update(model.PersonPrimaryEmail);
                        _dataUnitOfWork.BaseUow.PersonContactsRepository.Update(model.PersonPhone);

                        var contacts = _dataUnitOfWork.BaseUow.PersonContactsRepository.GetByPersonId(model.Person.Id).ToList();
                        if (model.PersonSecondaryEmail.Contact != null)
                        {
                            if (contacts.FirstOrDefault(x => x.ContactTypeId == (int)Enumerations.ContactTypes.SecondaryEmail) != null)
                            {
                                secondaryEmail = contacts.FirstOrDefault(x => x.ContactTypeId == (int)Enumerations.ContactTypes.SecondaryEmail);
                                secondaryEmail.Contact = model.PersonSecondaryEmail.Contact;
                                _dataUnitOfWork.BaseUow.PersonContactsRepository.Update(secondaryEmail);
                            }
                            else
                            {
                                model.PersonSecondaryEmail.PersonId = model.Person.Id;
                                model.PersonSecondaryEmail.ContactTypeId = (int)Enumerations.ContactTypes.SecondaryEmail;
                                _dataUnitOfWork.BaseUow.PersonContactsRepository.Add(model.PersonSecondaryEmail);
                            }

                        }
                        if (model.PersonMobile.Contact != null)
                        {
                            if (contacts.FirstOrDefault(x => x.ContactTypeId == (int)Enumerations.ContactTypes.MobileNumber) != null)
                            {
                                mobile = contacts.FirstOrDefault(x => x.ContactTypeId == (int)Enumerations.ContactTypes.MobileNumber);
                                mobile.Contact = model.PersonMobile.Contact;
                                _dataUnitOfWork.BaseUow.PersonContactsRepository.Update(mobile);
                            }
                            else
                            {
                                model.PersonMobile.PersonId = model.Person.Id;
                                _dataUnitOfWork.BaseUow.PersonContactsRepository.Add(model.PersonMobile);
                            }
                        }
                        _dataUnitOfWork.BaseUow.PersonsRepository.SaveChanges();

                        #region Log
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.Persons, model.Person.Id, GetControllerName(), GetActionName(), null);
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.PersonContacts, model.PersonPhone.Id, GetControllerName(), GetActionName(), null);
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.PersonContacts, model.PersonPrimaryEmail.Id, GetControllerName(), GetActionName(), null);
                        if (secondaryEmail != null)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.PersonContacts, secondaryEmail.Id, GetControllerName(), GetActionName(), null);
                        }
                        else
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.PersonContacts, model.PersonSecondaryEmail.Id, GetControllerName(), GetActionName(), null);
                        }
                        if (mobile != null)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.PersonContacts, mobile.Id, GetControllerName(), GetActionName(), null);
                        }
                        else
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.PersonContacts, model.PersonMobile.Id, GetControllerName(), GetActionName(), null);
                        }
                        #endregion

                        contextTransaction.Commit();

                        ViewData["SuccessStaffEditGeneralInfo"] = true;
                        return PartialView("_ProfileGeneralInfo", model);
                    }
                    catch (Exception ex)
                    {
                        contextTransaction.Rollback();

                        ModelState.AddModelError(nameof(Localizer.Error), Localizer.Error);
                        #region Log
                        _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Edit, Tables.Base.Persons, model.Person.Id, GetControllerName(), GetActionName(), ex);
                        _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Edit, Tables.Base.PersonContacts, model.PersonPhone.Id, GetControllerName(), GetActionName(), null);
                        _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Edit, Tables.Base.PersonContacts, model.PersonPrimaryEmail.Id, GetControllerName(), GetActionName(), null);
                        if (secondaryEmail != null)
                        {
                            _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Add, Tables.Base.PersonContacts, secondaryEmail.Id, GetControllerName(), GetActionName(), null);
                        }
                        else
                        {
                            _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Edit, Tables.Base.PersonContacts, model.PersonSecondaryEmail.Id, GetControllerName(), GetActionName(), null);
                        }
                        if (mobile != null)
                        {
                            _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Add, Tables.Base.PersonContacts, mobile.Id, GetControllerName(), GetActionName(), null);
                        }
                        else
                        {
                            _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Edit, Tables.Base.PersonContacts, model.PersonMobile.Id, GetControllerName(), GetActionName(), null);
                        }
                        #endregion
                    }
                }
            }
            return PartialView("_ProfileGeneralInfo", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditStatusInfo(StaffStatusInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (IDbContextTransaction contextTransaction = _dataUnitOfWork.BaseUow.PersonDetailsRepository.BeginTransaction())
                {
                    try
                    {
                        _dataUnitOfWork.BaseUow.PersonDetailsRepository.Update(model.PersonDetail);
                        _dataUnitOfWork.BaseUow.PersonDetailsRepository.SaveChanges();

                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.PersonDetails, model.PersonDetail.Id, GetControllerName(), GetActionName(), null);

                        contextTransaction.Commit();

                        ViewData["SuccessStaffEditStatusInfo"] = true;
                        return PartialView("_ProfileStatusInfo", model);
                    }
                    catch (Exception ex)
                    {
                        contextTransaction.Rollback();

                        ModelState.AddModelError(nameof(Localizer.Error), Localizer.Error);
                        _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Edit, Tables.Base.PersonDetails, model.PersonDetail.Id, GetControllerName(), GetActionName(), ex);
                    }
                }
            }
            return PartialView("_ProfileStatusInfo", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditAccountInfo(StaffAccountInfoViewModel model)
        {
            if (model.User.EditPassword != null)
            {
                var pattern = new Regex("(?=.*[0-9])(?=.*[a-zA-Z]).");
                if (!pattern.IsMatch(model.User.EditPassword))
                {
                    ModelState.AddModelError(nameof(Localizer.ErrorMessagePasswordReqContainLettersAndNumbers), Localizer.ErrorMessagePasswordReqContainLettersAndNumbers);
                }
                if (model.User.EditPassword.Count() < 6)
                {
                    ModelState.AddModelError(nameof(Localizer.ErrorMessagePasswordLen6), Localizer.ErrorMessagePasswordLen6);
                }
                if (model.User.EditPassword != model.User.ConfirmPassword)
                {
                    ModelState.AddModelError(nameof(Localizer.PasswordsDoNotMatch), Localizer.PasswordsDoNotMatch);
                }
            }
            if (ModelState.IsValid)
            {
                using (IDbContextTransaction contextTransaction = _dataUnitOfWork.BaseUow.InstitutionUsersRepository.BeginTransaction())
                {
                    var addedUserRoles = new List<int>();
                    var removedUserRoles = new List<int>();
                    try
                    {
                        var user = _dataUnitOfWork.BaseUow.UsersRepository.GetById(model.User.Id);

                        if (model.User.EditPassword != null)
                        {
                            user.PasswordSalt = Cryptography.Salt.Create();
                            user.PasswordHash = Cryptography.Hash.Create(model.User.EditPassword, user.PasswordSalt);
                            user.ChangedPassword = true;
                        }
                        user.Username = model.User.Username;
                        user.Email = model.User.Email;

                        _dataUnitOfWork.BaseUow.UsersRepository.Update(user);
                        _dataUnitOfWork.BaseUow.UsersRepository.SaveChanges();

                        var existingRoles = _dataUnitOfWork.BaseUow.UserRolesRepository.GetByInstitutionUserId(model.InstitutionUserId).ToList();
                        var newRoles = model.Roles.Select(x => int.Parse(x)).ToList();
                        foreach (var rola in existingRoles)
                        {
                            if (!newRoles.Contains(rola.Id))
                            {
                                _dataUnitOfWork.BaseUow.UserRolesRepository.Remove(rola);
                                removedUserRoles.Add(rola.Id);
                            }
                        }
                        foreach (var rola in newRoles)
                        {
                            if (!existingRoles.Select(x => x.Id).Contains(rola))
                            {
                                var userRola = new UserRole
                                {
                                    InstitutionUserId = model.InstitutionUserId,
                                    OrganizationInstitutionUserId=model.OrganizationInstitutionUserId,
                                    RoleId = rola
                                };
                                _dataUnitOfWork.BaseUow.UserRolesRepository.Add(userRola);
                                _dataUnitOfWork.BaseUow.UserRolesRepository.SaveChanges();
                                addedUserRoles.Add(userRola.Id);
                            }
                        }
                        _dataUnitOfWork.BaseUow.UserRolesRepository.SaveChanges();

                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.Users, model.User.Id, GetControllerName(), GetActionName(), null);
                        foreach (var roleId in addedUserRoles)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.UserRoles, roleId, GetControllerName(), GetActionName(), null);
                        }
                        foreach (var roleId in removedUserRoles)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.UserRoles, roleId, GetControllerName(), GetActionName(), null);
                        }


                        contextTransaction.Commit();

                        ViewData["SuccessStaffEditAccountInfo"] = true;
                        return PartialView("_ProfileAccountInfo", model);
                    }
                    catch (Exception ex)
                    {
                        contextTransaction.Rollback();

                        _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Edit, Tables.Base.Users, model.User.Id, GetControllerName(), GetActionName(), ex);
                        foreach (var roleId in addedUserRoles)
                        {
                            _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Add, Tables.Base.UserRoles, roleId, GetControllerName(), GetActionName(), ex);
                        }
                        foreach (var roleId in removedUserRoles)
                        {
                            _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.UserRoles, roleId, GetControllerName(), GetActionName(), ex);
                        }
                    }
                }
            }
            return PartialView("_ProfileAccountInfo", model);
        }
        public IActionResult EditProfileImage(int personId)
        {
            var model = new PersonViewModel { Id = personId };
            return PartialView("_EditProfileImage", model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditProfileImage(int Id, IFormFile profilePhoto)
        {
            var existPhoto = _dataUnitOfWork.BaseUow.PersonPhotosRepository.GetByPersonId(Id);
            PersonPhoto photo = null;
            try
            {
                if (profilePhoto.Length > 0)
                {
                    if (existPhoto != null)
                    {
                        existPhoto.Photo = _imageHelper.Save(profilePhoto);
                        existPhoto.Size = profilePhoto.Length;
                        existPhoto.Type = Path.GetExtension(profilePhoto.FileName);
                        existPhoto.Name = profilePhoto.Name;

                        _dataUnitOfWork.BaseUow.PersonPhotosRepository.Update(existPhoto);
                    }
                    else
                    {
                        photo = new PersonPhoto
                        {
                            Photo = _imageHelper.Save(profilePhoto),
                            Size = profilePhoto.Length,
                            Type = Path.GetExtension(profilePhoto.FileName),
                            Name = profilePhoto.Name,
                            PersonId = Id
                        };
                        _dataUnitOfWork.BaseUow.PersonPhotosRepository.Add(photo);
                    }
                    _dataUnitOfWork.BaseUow.PersonPhotosRepository.SaveChanges();
                    if (existPhoto != null)
                    {
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.PersonPhotos, existPhoto.Id, GetControllerName(), GetActionName(), null);
                    }
                    else
                    {
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.PersonPhotos, photo.Id, GetControllerName(), GetActionName(), null);
                    }
                    return Json(new { success = true, notification = new Notification(NotificationTypes.Success, Localizer.Saved, string.Format(Localizer.SuccessfullyChanged, Localizer.ProfileImage)).ConvertToJson() });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(Localizer.Error), Localizer.Error);
                if (existPhoto != null)
                {
                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.PersonPhotos, existPhoto.Id, GetControllerName(), GetActionName(), null);
                }
                else
                {
                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.PersonPhotos, photo.Id, GetControllerName(), GetActionName(), null);
                }
            }
            return Json(new { success = true, notification = new Notification(NotificationTypes.Error, Localizer.Error, Localizer.ErrorFriendly).ConvertToJson() });
        }
        #endregion

        #region Delete

        public IActionResult Delete(int Id, int InstitutionId)
        {
            Notification notification = null;
            using (IDbContextTransaction dbContextTransaction = _dataUnitOfWork.BaseUow.PersonsRepository.BeginTransaction())
            {
                InstitutionUser institutionUser = null;
                try
                {
                    //base 
                    institutionUser = _dataUnitOfWork.BaseUow.InstitutionUsersRepository.GetByUserIdAndInstitutionId(Id, InstitutionId);
                    var organizationInstitutionUser = _dataUnitOfWork.BaseUow.OrganizationInstitutionUsersRepository.GetByInstitutionUserId(institutionUser.Id);
                    var user = _dataUnitOfWork.BaseUow.UsersRepository.GetById(Id);
                    var member = _dataUnitOfWork.BaseUow.MembersRepository.GetById(institutionUser.Id);
                    MemberLicense license = null;
                    List<LicensePeriod> licensePeriods = null;
                    if (member != null)
                    {
                        license = _dataUnitOfWork.BaseUow.MemberLicensesRepository.GetByMemberId(member.Id);
                        if (!license.Permanent)
                        {
                            licensePeriods = _dataUnitOfWork.BaseUow.LicensePeriodsRepository.GetByLicenseId(license.Id).ToList();
                            _dataUnitOfWork.BaseUow.LicensePeriodsRepository.RemoveRange(licensePeriods);
                        }
                        _dataUnitOfWork.BaseUow.MemberLicensesRepository.Remove(license, true);
                        _dataUnitOfWork.BaseUow.MembersRepository.Remove(member, true);
                    }
                    var userRoles = _dataUnitOfWork.BaseUow.UserRolesRepository.GetByInstitutionUserId(institutionUser.Id);
                    _dataUnitOfWork.BaseUow.UserRolesRepository.RemoveRange(userRoles);

                    if (organizationInstitutionUser != null)
                    {
                        _dataUnitOfWork.BaseUow.OrganizationInstitutionUsersRepository.Remove(organizationInstitutionUser, true);
                    }
                    _dataUnitOfWork.BaseUow.InstitutionUsersRepository.Remove(institutionUser, true);

                    _dataUnitOfWork.BaseUow.UsersRepository.Remove(user, true);
                    _dataUnitOfWork.BaseUow.UsersRepository.SaveChanges();

                    // 
                    var person = _dataUnitOfWork.BaseUow.PersonsRepository.GetById(Id);
                    var personPhoto = _dataUnitOfWork.BaseUow.PersonPhotosRepository.GetByPersonId(Id);
                    var personDetails = _dataUnitOfWork.BaseUow.PersonDetailsRepository.GetById(Id);
                    var personContacts = _dataUnitOfWork.BaseUow.PersonContactsRepository.GetByPersonId(Id);

                    _dataUnitOfWork.BaseUow.PersonContactsRepository.RemoveRange(personContacts);
                    _dataUnitOfWork.BaseUow.PersonDetailsRepository.Remove(personDetails);
                    if (personPhoto != null)
                    {
                        _dataUnitOfWork.BaseUow.PersonPhotosRepository.Remove(personPhoto);
                    }
                    _dataUnitOfWork.BaseUow.PersonsRepository.Remove(person);
                    _dataUnitOfWork.BaseUow.PersonsRepository.SaveChanges();


                    #region Log
                    if (member != null)
                    {
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.Members, member.Id, GetControllerName(), GetActionName(), null);
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.MemberLicenses, license.Id, GetControllerName(), GetActionName(), null);
                        if (!license.Permanent)
                        {
                            foreach (var licensePeriod in licensePeriods)
                            {
                                _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.LicensePeriods, licensePeriod.Id, GetControllerName(), GetActionName(), null);
                            }
                        }
                    }
                    if (organizationInstitutionUser != null)
                    {
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.OrganizationInstitutionUsers, organizationInstitutionUser.Id, GetControllerName(), GetActionName(), null);
                    }
                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.InstitutionUsers, institutionUser.Id, GetControllerName(), GetActionName(), null);
                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.Users, user.Id, GetControllerName(), GetActionName(), null);
                    foreach (var role in userRoles)
                    {
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.UserRoles, role.Id, GetControllerName(), GetActionName(), null);
                    }


                    //
                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.Persons, person.Id, GetControllerName(), GetActionName(), null);
                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.PersonDetails, personDetails.Id, GetControllerName(), GetActionName(), null);
                    foreach (var contact in personContacts)
                    {
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.PersonContacts, contact.Id, GetControllerName(), GetActionName(), null);
                    }
                    if (personPhoto != null)
                    {
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.PersonPhotos, personPhoto.Id, GetControllerName(), GetActionName(), null);

                    }
                    #endregion

                    notification = new Notification(NotificationTypes.Success, Localizer.Removed, string.Format(Localizer.SuccessfullyRemovedName, Localizer.Staff));

                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    notification = new Notification(NotificationTypes.Error, _localizer.ErrorFriendly, _localizer.AnErrorOccurredFriendly);
                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.Persons, institutionUser.UserId, GetControllerName(), GetActionName(), ex);

                }
            }
            return Json(notification.ConvertToJson());
        }

        #endregion

        #region Profile

        public IActionResult Profile(int Id, int InstitutionId)
        {
            var model = new StaffViewModel();

            var institutionUser = _dataUnitOfWork.BaseUow.InstitutionUsersRepository.GetByUserIdAndInstitutionId(Id, InstitutionId);
            var organizationInstitutitonUser = _dataUnitOfWork.BaseUow.OrganizationInstitutionUsersRepository.GetByInstitutionUserId(institutionUser.Id);
            if (organizationInstitutitonUser != null)
            {
                model.Member = _dataUnitOfWork.BaseUow.MembersRepository.GetById(organizationInstitutitonUser.Id) != null ? true : false;
                if (model.Member)
                {
                    var license = _dataUnitOfWork.BaseUow.MemberLicensesRepository.GetByMemberId(organizationInstitutitonUser.Id);
                    if (license != null)
                    {
                        model.License = license;
                    }
                }
            }

            model.PersonPhoto = _dataUnitOfWork.BaseUow.PersonPhotosRepository.GetByPersonId(Id);
            if (model.PersonPhoto != null)
            {
                model.PersonPhotoBase64 = string.Format("data:image/png|jpg;base64,{0}", Convert.ToBase64String(model.PersonPhoto.Photo));
            }

            var contacts = _dataUnitOfWork.BaseUow.PersonContactsRepository.GetByPersonId(Id).ToList();
            var personPhone = contacts.FirstOrDefault(x => x.ContactTypeId == (int)Enumerations.ContactTypes.PhoneNumber) != null ? contacts.FirstOrDefault(x => x.ContactTypeId == (int)Enumerations.ContactTypes.PhoneNumber) : null;
            var primaryEmail = contacts.FirstOrDefault(x => x.ContactTypeId == (int)Enumerations.ContactTypes.PrimaryEmail) != null ? contacts.FirstOrDefault(x => x.ContactTypeId == (int)Enumerations.ContactTypes.PrimaryEmail) : null;

            model.GeneralInfo = new StaffGeneralInfoViewModel
            {
                Person = _dataUnitOfWork.BaseUow.PersonsRepository.GetById(Id),
                PersonPhone = personPhone,
                PersonPrimaryEmail = primaryEmail
            };
            if (contacts.FirstOrDefault(x => x.ContactTypeId == (int)Enumerations.ContactTypes.MobileNumber) != null)
                model.GeneralInfo.PersonMobile = contacts.FirstOrDefault(x => x.ContactTypeId == (int)Enumerations.ContactTypes.MobileNumber);
            if (contacts.FirstOrDefault(x => x.ContactTypeId == (int)Enumerations.ContactTypes.SecondaryEmail) != null)
                model.GeneralInfo.PersonSecondaryEmail = contacts.FirstOrDefault(x => x.ContactTypeId == (int)Enumerations.ContactTypes.SecondaryEmail);

            model.StatusInfo = new StaffStatusInfoViewModel
            {
                PersonDetail = _dataUnitOfWork.BaseUow.PersonDetailsRepository.GetById(Id),
                InstitutionId = institutionUser.InstitutionId
            };
            model.AccountInfo = new StaffAccountInfoViewModel
            {
                Roles = _dataUnitOfWork.BaseUow.UserRolesRepository.GetByInstitutionUserId(institutionUser.Id).Select(x => x.RoleId.ToString()).ToList() ,
                User = _dataUnitOfWork.BaseUow.UsersRepository.GetById(Id),
                InstitutionUserId = institutionUser.Id,
                OrganizationInstitutionUserId=organizationInstitutitonUser?.Id
            };
            return PartialView(MagicStrings.ViewNames.Preview, model);
        }
        //public IActionResult Profile(int Id, int InstitutionId)
        //{
        //    var model = new StaffViewModel();
        //    model.GeneralInfo.Person = _dataUnitOfWork.BaseUow.PersonsRepository.GetById(Id);
        //    model.StatusInfo.PersonDetail = _dataUnitOfWork.BaseUow.PersonDetailsRepository.GetById(Id);
        //    model.AccountInfo.User = _dataUnitOfWork.BaseUow.UsersRepository.GetById(Id);
        //    var institutionUser = _dataUnitOfWork.BaseUow.InstitutionUsersRepository.GetByUserIdAndInstitutionId(Id, InstitutionId);
        //    model.AccountInfo.Roles = _dataUnitOfWork.BaseUow.UserRolesRepository.GetByInstitutionUserId(institutionUser.Id).Select(x => x.RoleId.ToString()).ToList();
        //    model.PersonPhoto = _dataUnitOfWork.BaseUow.PersonPhotosRepository.GetByPersonId(Id);
        //    if (model.PersonPhoto != null)
        //    {
        //        model.PersonPhotoBase64 = string.Format("data:image/png|jpg;base64,{0}", Convert.ToBase64String(model.PersonPhoto.Photo));
        //    }
        //    var contacts = _dataUnitOfWork.BaseUow.PersonContactsRepository.GetByPersonId(Id).ToList();
        //    if (contacts.FirstOrDefault(x => x.ContactTypeId == (int)Enumerations.ContactTypes.MobileNumber) != null)
        //        model.GeneralInfo.PersonMobile = contacts.FirstOrDefault(x => x.ContactTypeId == (int)Enumerations.ContactTypes.MobileNumber);
        //    if (contacts.FirstOrDefault(x => x.ContactTypeId == (int)Enumerations.ContactTypes.SecondaryEmail) != null)
        //        model.GeneralInfo.PersonSecondaryEmail = contacts.FirstOrDefault(x => x.ContactTypeId == (int)Enumerations.ContactTypes.SecondaryEmail);
        //    model.GeneralInfo.PersonPhone = contacts.FirstOrDefault(x => x.ContactTypeId == (int)Enumerations.ContactTypes.PhoneNumber) != null ? contacts.FirstOrDefault(x => x.ContactTypeId == (int)Enumerations.ContactTypes.PhoneNumber) : null;
        //    model.GeneralInfo.PersonPrimaryEmail = contacts.FirstOrDefault(x => x.ContactTypeId == (int)Enumerations.ContactTypes.PrimaryEmail) != null ? contacts.FirstOrDefault(x => x.ContactTypeId == (int)Enumerations.ContactTypes.PrimaryEmail) : null;

        //    model.GeneralInfo = new GeneralInfoViewModel { Person = model.GeneralInfo.Person, PersonMobile = model.PersonMobile, PersonPhone = model.PersonPhone, PersonPrimaryEmail = model.PersonPrimaryEmail, PersonSecondaryEmail = model.PersonSecondaryEmail };
        //    model.StatusInfo = new StatusInfoViewModel {   InstitutionId = InstitutionId };
        //    model.AccountInfo = new AccountInfoViewModel { Roles = model.Roles, User = model.User, InstitutionUserId = institutionUser.Id };
        //    return PartialView(MagicStrings.ViewNames.Preview, model);
        //}
        #endregion
    }
}