using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace nevladinaOrg.Web.Areas.Organizations.Controllers
{
    [Area(MagicStrings.AreaNames.Organizations)]
    [Authorization]
    public class MembersController : BaseController
    {
        #region Properties
        IDataUnitOfWork _dataUnitOfWork;

        private Localizer _localizer;
        private ILogger _logger;
        private IImageHelper _imageHelper;
        #endregion
        public MembersController(IDataUnitOfWork dataUnitOfWork,
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
        [HttpGet]
        public JsonResult JsonIndex()
        {
            //var institutionUsersDTO = _dataUnitOfWork.BaseUow.InstitutionUsersDTORepository.GetByInstitutionId(LoggedUserIds.InstitutionId).ToList();


            //List<Tuple<int, int, int>> institutionUsersIds = institutionUsersDTO.Select(x => new Tuple<int, int, int>(x.Id, x.InstitutionId, x.UserId)).ToList();
            //var personsDTO = _dataUnitOfWork.BaseUow.PersonsDTORepository.GetByUserIds(institutionUsersIds.Select(x => x.Item3).ToList()).ToList();
            //foreach (var person in personsDTO)
            //{
            //    person.InstitutionUserId = institutionUsersIds.FirstOrDefault(x => x.Item3 == person.Id).Item1;
            //    person.InstitutionId = institutionUsersIds.FirstOrDefault(x => x.Item3 == person.Id).Item2;
            //}

            //var institutionUsersDTOJson = JsonConvert.SerializeObject(personsDTO);
            //return new JsonResult(institutionUsersDTOJson);

            IEnumerable<OrganizationInstitutionUserDTO> organizationInstitutionUsersDTO = null;
            List<PersonDTO> personsDTO = null;
            if (LoggedUserIds.OrganizationId.HasValue)
            {
                organizationInstitutionUsersDTO = _dataUnitOfWork.BaseUow.OrganizationInstitutionUsersDTORepository.GetAllByOrganizationId(LoggedUserIds.OrganizationId.Value).ToList();   //LoggedUser.organizationID 
            }
            if (organizationInstitutionUsersDTO != null)
            {
                //var users = _personsAuthRepository.GetByUserIds(organizationInstitutionUsersDTO.Select(x => x.UserId).ToList());
                //foreach (var institutionUser in organizationInstitutionUsersDTO)
                //{
                //    institutionUser.User = users.FirstOrDefault(x => x.Id == institutionUser.UserId).FirstName + " " + users.FirstOrDefault(x => x.Id == institutionUser.UserId).LastName;
                //}
                //OrganizationInstitutionUserId,InstiuttionUserId,UserId
                List<Tuple<int, int, int>> organizationInstitutionUsersIds = organizationInstitutionUsersDTO.Select(x => new Tuple<int, int, int>(x.Id, x.InstitutionUserId, x.UserId)).ToList();
                personsDTO = _dataUnitOfWork.BaseUow.PersonsDTORepository.GetByUserIds(organizationInstitutionUsersIds.Select(x => x.Item3).ToList()).ToList();
                foreach (var person in personsDTO)
                {
                    person.OrganizationInstitutionUserId = organizationInstitutionUsersIds.FirstOrDefault(x => x.Item3 == person.Id).Item1;
                    person.InstitutionUserId = organizationInstitutionUsersIds.FirstOrDefault(x => x.Item3 == person.Id).Item2;
                }

            }
            var personsDTOJson = JsonConvert.SerializeObject(personsDTO);
            return new JsonResult(personsDTOJson);
        }
        public JsonResult JsonIndexEvents(int PersonId)
        {
            IEnumerable<EventDTO> events = _dataUnitOfWork.BaseUow.EventsDTORepository.GetByUserId(PersonId);

            var eventsJson = JsonConvert.SerializeObject(events);

            return new JsonResult(eventsJson);
        }

        #endregion

        #region Add

        [HttpGet]
        public IActionResult Add()
        {
            var model = new MemberViewModel()
            {
                Person = new PersonViewModel(),
                PersonDetail = new PersonDetailsViewModel(),
                User = new UserViewModel(),
                License = new MemberLicenseViewModel(),
                LicensePeriod = new LicensePeriodViewModel(),
                Member = true,
                LicenceTypePeriod = "perPeriod",
                Roles = new List<string>(),
                PersonMobile = new PersonContact() { ContactTypeId = (int)Enumerations.ContactTypes.MobileNumber },
                PersonPhone = new PersonContact() { ContactTypeId = (int)Enumerations.ContactTypes.PhoneNumber },
                PersonPrimaryEmail = new PersonContact() { ContactTypeId = (int)Enumerations.ContactTypes.PrimaryEmail },
                PersonSecondaryEmail = new PersonContact() { ContactTypeId = (int)Enumerations.ContactTypes.SecondaryEmail }

            };
            return PartialView(MagicStrings.ViewNames._Add, model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Add(MemberViewModel model)
        {
            if (!model.Employer && !model.Member)
            {
                ModelState.AddModelError(nameof(Localizer.SelectMembershipType), Localizer.SelectMembershipType);
            }
            if (model.PersonPhone.Contact == null)
            {
                ModelState.AddModelError(nameof(Localizer.ErrorMessageContactPhoneReq), Localizer.ErrorMessageContactPhoneReq);
            }
            if (model.PersonPrimaryEmail.Contact == null)
            {
                ModelState.AddModelError(nameof(Localizer.ErrorMessagePrimaryEmailReq), Localizer.ErrorMessagePrimaryEmailReq);
            }
            if (model.Member)
            {
                if (model.License.LicenceNumber == null)
                    ModelState.AddModelError(nameof(Localizer.ErrorMessageLicenseNumberReq), Localizer.ErrorMessageLicenseNumberReq);
                if (model.LicenceTypePeriod == "perPeriod" && (model.LicensePeriod.CreatedDate == null || model.LicensePeriod.EndDate == null))
                {
                    ModelState.AddModelError(nameof(Localizer.ErrorMessageStartDateReq), Localizer.ErrorMessageStartDateReq);
                    ModelState.AddModelError(nameof(Localizer.ErrorMessageEndDateReq), Localizer.ErrorMessageEndDateReq);
                }
            }
            var pattern = new Regex("(?=.*[0-9])(?=.*[a-zA-Z]).");
            if (!pattern.IsMatch(model.User.Password))
            {
                ModelState.AddModelError(nameof(Localizer.ErrorMessagePasswordReqContainLettersAndNumbers), Localizer.ErrorMessagePasswordReqContainLettersAndNumbers);
            }
            if (model.User.Password.Count() < 6)
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

                using (IDbContextTransaction contextTransaction = _dataUnitOfWork.BaseUow.MembersRepository.BeginTransaction())
                {
                    model.Person.CitizenshipId = model.Person.CitizenshipId == 0 ? null : model.Person.CitizenshipId;
                    model.Person.BirthCityId = model.Person.BirthCityId == 0 ? null : model.Person.BirthCityId;
                    model.Person.RegionId = model.Person.RegionId == 0 ? null : model.Person.RegionId;
                    model.Person.BirthCountryId = model.Person.BirthCountryId == 0 ? null : model.Person.BirthCountryId;
                    model.Person.ResidenceId = model.Person.ResidenceId == 0 ? null : model.Person.ResidenceId;
                    model.PersonDetail.MartialStatusId = model.PersonDetail.MartialStatusId == 0 ? null : model.PersonDetail.MartialStatusId;
                    model.PersonDetail.AcademicTitleId = model.PersonDetail.AcademicTitleId == 0 ? null : model.PersonDetail.AcademicTitleId;
                    model.PersonDetail.EmploymentStatusId = model.PersonDetail.EmploymentStatusId == 0 ? null : model.PersonDetail.EmploymentStatusId;

                    var user = new User();
                    user = model.User;

                    var person = new Person();
                    person = model.Person;

                    var personDetails = new PersonDetail();
                    personDetails = model.PersonDetail;

                    var license = new MemberLicense();
                    license = model.License;

                    var licensePeriod = new LicensePeriod();
                    licensePeriod = model.LicensePeriod;


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
                        user.PasswordHash = Cryptography.Hash.Create(model.User.Password, user.PasswordSalt);
                        _dataUnitOfWork.BaseUow.UsersRepository.Add(user);
                        _dataUnitOfWork.BaseUow.UsersRepository.SaveChanges();

                        // add model class object
                        model.PersonPrimaryEmail.PersonId = person.Id;
                        model.PersonPhone.PersonId = person.Id;
                        _dataUnitOfWork.BaseUow.PersonContactsRepository.Add(model.PersonPrimaryEmail);
                        _dataUnitOfWork.BaseUow.PersonContactsRepository.Add(model.PersonPhone);

                        if (model.PersonSecondaryEmail.Contact != null)
                        {
                            model.PersonSecondaryEmail.PersonId = person.Id;
                            _dataUnitOfWork.BaseUow.PersonContactsRepository.Add(model.PersonSecondaryEmail);
                        }
                        if (model.PersonMobile.Contact != null)
                        {
                            model.PersonMobile.PersonId = person.Id;
                            _dataUnitOfWork.BaseUow.PersonContactsRepository.Add(model.PersonMobile);
                        }

                        var files = HttpContext.Request.Form.Files;
                        var photo = new PersonPhoto();
                        if (files.Count > 0)
                        {
                            if (files[0].Length > 0)
                            {
                                photo.PersonId = person.Id;
                                photo.Photo = _imageHelper.Save(files[0]);
                            }
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

                        var organizationInstitutionUser = new OrganizationInstitutionUser
                        {
                            Active=true,
                            Employed=model.Employer,
                            InstitutionUserId=institutionUser.Id,
                            OrganizationId = LoggedUserIds.OrganizationId.GetValueOrDefault()
                        };
                        _dataUnitOfWork.BaseUow.OrganizationInstitutionUsersRepository.Add(organizationInstitutionUser);
                        _dataUnitOfWork.BaseUow.OrganizationInstitutionUsersRepository.SaveChanges();

                        foreach (var role in model.Roles)
                        {
                            var userRole = new UserRole()
                            {
                                InstitutionUserId = institutionUser.Id,
                                OrganizationInstitutionUserId=organizationInstitutionUser.Id,
                                RoleId = int.Parse(role),
                            };
                            _dataUnitOfWork.BaseUow.UserRolesRepository.Add(userRole);
                        }
                        _dataUnitOfWork.BaseUow.UserRolesRepository.SaveChanges();

                        var member = new Member()
                        {
                            Id = organizationInstitutionUser.Id,
                            Note = model.Note
                        };
                        if (model.Member)
                        {
                            _dataUnitOfWork.BaseUow.MembersRepository.Add(member);
                            _dataUnitOfWork.BaseUow.MembersRepository.SaveChanges();

                            license.MemberId = member.Id;
                            license.Active = true;
                            license.CreatedDate = DateTime.Now;
                            license.Permanent = model.LicenceTypePeriod == "permanent" ? true : false;
                            _dataUnitOfWork.BaseUow.MemberLicensesRepository.Add(license);
                            _dataUnitOfWork.BaseUow.MemberLicensesRepository.SaveChanges();

                            if (!license.Permanent)
                            {
                                licensePeriod.MemberLicenseId = license.Id;
                                licensePeriod.LicenseTypeId = 1;
                                licensePeriod.Active = true;
                                _dataUnitOfWork.BaseUow.LicensePeriodsRepository.Add(licensePeriod);
                                _dataUnitOfWork.BaseUow.LicensePeriodsRepository.SaveChanges();
                            }
                        }



                        contextTransaction.Commit();

                        #region Logiranje
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.Persons, person.Id, GetControllerName(), GetActionName(), null);
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.Users, user.Id, GetControllerName(), GetActionName(), null);
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.PersonDetails, personDetails.Id, GetControllerName(), GetActionName(), null);
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.PersonContacts, model.PersonPrimaryEmail.Id, GetControllerName(), GetActionName(), null);
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.PersonContacts, model.PersonPhone.Id, GetControllerName(), GetActionName(), null);
                        if (model.PersonMobile.Contact != null)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.PersonContacts, model.PersonMobile.Id, GetControllerName(), GetActionName(), null);
                        }
                        if (model.PersonSecondaryEmail.Contact != null)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.PersonContacts, model.PersonMobile.Id, GetControllerName(), GetActionName(), null);
                        }
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.InstitutionUsers, institutionUser.Id, GetControllerName(), GetActionName(), null);
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.OrganizationInstitutionUsers, organizationInstitutionUser.Id, GetControllerName(), GetActionName(), null);
                        if (model.Member)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.Members, member.Id, GetControllerName(), GetActionName(), null);
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.MemberLicenses, license.Id, GetControllerName(), GetActionName(), null);
                            if (!license.Permanent)
                            {
                                _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.LicensePeriods, licensePeriod.Id, GetControllerName(), GetActionName(), null);
                            }
                        }
                        #endregion
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        contextTransaction.Rollback();

                        ModelState.AddModelError(nameof(Localizer.Error), Localizer.Error);
                        _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Add, Tables.Base.Members, user.Id, GetControllerName(), GetActionName(), ex);
                    }
                }
            }
            return PartialView(MagicStrings.ViewNames._Add, model);
        }

        #endregion

        #region Edit

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditGeneralInfo(GeneralInfoViewModel model)
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

                        ViewData["SuccessEditGeneralInfo"] = true;
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
        public IActionResult EditStatusInfo(StatusInfoViewModel model)
        {
            if (!model.Employer && !model.Member)
            {
                ModelState.AddModelError(nameof(Localizer.SelectMembershipType), Localizer.SelectMembershipType);
            }
            if (ModelState.IsValid)
            {
                using (IDbContextTransaction contextTransaction = _dataUnitOfWork.BaseUow.InstitutionUsersRepository.BeginTransaction())
                {
                    InstitutionUser institutionUser = null;
                    OrganizationInstitutionUser organizationInstitutionUser = null;
                    try
                    {
                        institutionUser = _dataUnitOfWork.BaseUow.InstitutionUsersRepository.GetByUserIdAndInstitutionId(model.PersonDetail.Id, model.InstitutionId);
                        organizationInstitutionUser = _dataUnitOfWork.BaseUow.OrganizationInstitutionUsersRepository.GetByInstitutionUserId(institutionUser.Id);
                        organizationInstitutionUser.Employed = model.Employer;

                        _dataUnitOfWork.BaseUow.OrganizationInstitutionUsersRepository.Update(organizationInstitutionUser);

                        _dataUnitOfWork.BaseUow.PersonDetailsRepository.Update(model.PersonDetail);

                        _dataUnitOfWork.BaseUow.OrganizationInstitutionUsersRepository.SaveChanges();
                        _dataUnitOfWork.BaseUow.PersonDetailsRepository.SaveChanges();

                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.OrganizationInstitutionUsers, institutionUser.Id, GetControllerName(), GetActionName(), null);
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.PersonDetails, model.PersonDetail.Id, GetControllerName(), GetActionName(), null);

                        contextTransaction.Commit();

                        ViewData["SuccessEditStatusInfo"] = true;
                        return PartialView("_ProfileStatusInfo", model);
                    }
                    catch (Exception ex)
                    {
                        contextTransaction.Rollback();

                        ModelState.AddModelError(nameof(Localizer.Error), Localizer.Error);
                        if (institutionUser != null)
                        {
                            _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Edit, Tables.Base.InstitutionUsers, institutionUser.Id, GetControllerName(), GetActionName(), ex);
                        }
                        _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Edit, Tables.Base.PersonDetails, model.PersonDetail.Id, GetControllerName(), GetActionName(), ex);
                    }
                }
            }
            return PartialView("_ProfileStatusInfo", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditAccountInfo(AccountInfoViewModel model)
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

                        var existingRoles = _dataUnitOfWork.BaseUow.UserRolesRepository.GetByInstitutionUserId(model.InstitutionUserId);
                        var newRoles = model.Roles.Select(x => int.Parse(x));
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

                        ViewData["SuccessEditAccountInfo"] = true;
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
        [HttpPost, IgnoreAntiforgeryToken]
        public IActionResult EditProfileImage(int Id, IFormFile profilePhoto)
        {
            var thumbnailFileBase64 = "";
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
                        thumbnailFileBase64 = $"data:{profilePhoto.ContentType};base64, {Convert.ToBase64String(existPhoto.Photo)}";

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
                        thumbnailFileBase64 = $"data:{profilePhoto.ContentType};base64, {Convert.ToBase64String(photo.Photo)}";
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
                    return Json(new { success = true, notification = new Notification(NotificationTypes.Success, Localizer.Saved, string.Format(Localizer.SuccessfullyChanged, Localizer.ProfileImage)).ConvertToJson(), thumbnailFileBase64 });
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

        #region Preview
        public IActionResult PreviewProfile(int Id)
        {
            var model = new MemberViewModel();
            return PartialView(MagicStrings.ViewNames._Preview, model);
        }
        public IActionResult Profile(int OrganizationInstitutionUserId)
        {
            var organizationInstitutionUser = _dataUnitOfWork.BaseUow.OrganizationInstitutionUsersRepository.GetById(OrganizationInstitutionUserId);
            var institutionUser = _dataUnitOfWork.BaseUow.InstitutionUsersRepository.GetById(organizationInstitutionUser.InstitutionUserId);
            var model = new MemberViewModel();
            model.Person = _dataUnitOfWork.BaseUow.PersonsRepository.GetById(institutionUser.UserId);
            model.PersonDetail = _dataUnitOfWork.BaseUow.PersonDetailsRepository.GetById(institutionUser.UserId);
            model.User = _dataUnitOfWork.BaseUow.UsersRepository.GetById(institutionUser.UserId);
            model.Roles = _dataUnitOfWork.BaseUow.UserRolesRepository.GetByOrganizationInstitutionUserId(OrganizationInstitutionUserId).Select(x => x.RoleId.ToString()).ToList();
            model.Member = _dataUnitOfWork.BaseUow.MembersRepository.GetById(organizationInstitutionUser.Id) != null ? true : false;
            model.Employer = _dataUnitOfWork.BaseUow.OrganizationInstitutionUsersRepository.GetByInstitutionUserId(institutionUser.Id).Employed;
            model.PersonPhoto = _dataUnitOfWork.BaseUow.PersonPhotosRepository.GetByPersonId(institutionUser.UserId);
            if (model.Member)
            {
                var license = _dataUnitOfWork.BaseUow.MemberLicensesRepository.GetByMemberId(organizationInstitutionUser.Id);
                if (license != null)
                {
                    model.License = license;
                }
            }
            if (model.PersonPhoto != null)
            {
                model.PersonPhotoBase64 = string.Format("data:image/png|jpg;base64,{0}", Convert.ToBase64String(model.PersonPhoto.Photo));
            }
            var contacts = _dataUnitOfWork.BaseUow.PersonContactsRepository.GetByPersonId(institutionUser.UserId).ToList();
            if (contacts.FirstOrDefault(x => x.ContactTypeId == (int)Enumerations.ContactTypes.MobileNumber) != null)
                model.PersonMobile = contacts.FirstOrDefault(x => x.ContactTypeId == (int)Enumerations.ContactTypes.MobileNumber);
            if (contacts.FirstOrDefault(x => x.ContactTypeId == (int)Enumerations.ContactTypes.SecondaryEmail) != null)
                model.PersonSecondaryEmail = contacts.FirstOrDefault(x => x.ContactTypeId == (int)Enumerations.ContactTypes.SecondaryEmail);
            model.PersonPhone = contacts.FirstOrDefault(x => x.ContactTypeId == (int)Enumerations.ContactTypes.PhoneNumber) != null ? contacts.FirstOrDefault(x => x.ContactTypeId == (int)Enumerations.ContactTypes.PhoneNumber) : null;
            model.PersonPrimaryEmail = contacts.FirstOrDefault(x => x.ContactTypeId == (int)Enumerations.ContactTypes.PrimaryEmail) != null ? contacts.FirstOrDefault(x => x.ContactTypeId == (int)Enumerations.ContactTypes.PrimaryEmail) : null;

            model.GeneralInfo = new GeneralInfoViewModel { Person = model.Person, PersonMobile = model.PersonMobile, PersonPhone = model.PersonPhone, PersonPrimaryEmail = model.PersonPrimaryEmail, PersonSecondaryEmail = model.PersonSecondaryEmail };
            model.StatusInfo = new StatusInfoViewModel { Employer = model.Employer, Member = model.Member, PersonDetail = model.PersonDetail, InstitutionId = institutionUser.InstitutionId };
            model.AccountInfo = new AccountInfoViewModel { Roles = model.Roles, User = model.User, InstitutionUserId = institutionUser.Id };
            return PartialView(MagicStrings.ViewNames.Preview, model);
        }
        #endregion

        #region Delete

        public IActionResult Delete(int Id, int InstitutionId)
        {
            Notification notification = null;
            using (IDbContextTransaction dbContextTransaction = _dataUnitOfWork.BaseUow.PersonsRepository.BeginTransaction())
            {
                try
                {
                    //base 
                    var institutionUser = _dataUnitOfWork.BaseUow.InstitutionUsersRepository.GetByUserIdAndInstitutionId(Id, InstitutionId);
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
                    _dataUnitOfWork.BaseUow.UsersRepository.Remove(user);
                    if (personPhoto != null)
                    {
                        _dataUnitOfWork.BaseUow.PersonPhotosRepository.Remove(personPhoto);
                    }
                    _dataUnitOfWork.BaseUow.PersonsRepository.Remove(person);
                    _dataUnitOfWork.BaseUow.PersonsRepository.SaveChanges();


                    #region Log
                    //base
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
                    dbContextTransaction.Rollback();

                    notification = new Notification(NotificationTypes.Error, _localizer.ErrorFriendly, _localizer.AnErrorOccurredFriendly);
                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.Users, Id, GetControllerName(), GetActionName(), ex);
                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.Persons, Id, GetControllerName(), GetActionName(), ex);

                }
            }
            return Json(notification.ConvertToJson());
        }
        public IActionResult DeleteAll(List<int> list)
        {
            List<City> modelList = new List<City>();
            Notification notification = null;
            using (IDbContextTransaction dbContextTransaction = _dataUnitOfWork.BaseUow.PersonsRepository.BeginTransaction())
            {
                    try
                    {
                        //
                        var removedContacts = new List<List<int>>();
                        var removedPersonDetails = new List<int>();
                        var removedUsers = new List<int>();
                        var removedPersons = new List<int>();
                        var removedPhotos = new List<int>();
                        InstitutionUser tempInstitutionUser = null;

                        foreach (var Id in list)
                        {
                            tempInstitutionUser = _dataUnitOfWork.BaseUow.InstitutionUsersRepository.GetById(Id);

                            var personContacts = _dataUnitOfWork.BaseUow.PersonContactsRepository.GetByPersonId(tempInstitutionUser.UserId).ToList();
                            _dataUnitOfWork.BaseUow.PersonContactsRepository.RemoveRange(personContacts);
                            removedContacts.Add(personContacts.Select(x => x.Id).ToList());

                            removedPersonDetails.Add(_dataUnitOfWork.BaseUow.PersonDetailsRepository.Remove(tempInstitutionUser.UserId));

                            removedUsers.Add(_dataUnitOfWork.BaseUow.UsersRepository.Remove(tempInstitutionUser.UserId));

                            var personPhoto = _dataUnitOfWork.BaseUow.PersonPhotosRepository.GetByPersonId(tempInstitutionUser.UserId);
                            if (personPhoto != null)
                            {
                                _dataUnitOfWork.BaseUow.PersonPhotosRepository.Remove(personPhoto);
                                removedPhotos.Add(personPhoto.Id);
                            }
                            _dataUnitOfWork.BaseUow.PersonsRepository.Remove(_dataUnitOfWork.BaseUow.PersonsRepository.GetById(tempInstitutionUser.UserId));
                            removedPersons.Add(tempInstitutionUser.UserId);
                        }
                        _dataUnitOfWork.BaseUow.PersonsRepository.SaveChanges();

                        //base
                        var removedLicensePeriods = new List<List<int>>();
                        var removedLicenses = new List<int>();
                        var removedMembers = new List<int>();
                        var removedUserRoles = new List<List<int>>();
                        var removedOrganizationInstitutionUsers = new List<int>();
                        var removedInstitutionUsers = new List<int>();

                        foreach (var Id in list)
                        {
                            var member = _dataUnitOfWork.BaseUow.MembersRepository.GetById(Id);
                            MemberLicense license = null;
                            List<LicensePeriod> licensePeriods = null;
                            if (member != null)
                            {
                                license = _dataUnitOfWork.BaseUow.MemberLicensesRepository.GetByMemberId(member.Id);
                                if (!license.Permanent)
                                {
                                    licensePeriods = _dataUnitOfWork.BaseUow.LicensePeriodsRepository.GetByLicenseId(license.Id).ToList();
                                    _dataUnitOfWork.BaseUow.LicensePeriodsRepository.RemoveRange(licensePeriods);
                                    removedLicensePeriods.Add(licensePeriods.Select(x => x.Id).ToList());
                                }
                                _dataUnitOfWork.BaseUow.MemberLicensesRepository.Remove(license, true);
                                removedLicenses.Add(license.Id);

                                _dataUnitOfWork.BaseUow.MembersRepository.Remove(member, true);
                                removedMembers.Add(member.Id);
                            }
                            var userRoles = _dataUnitOfWork.BaseUow.UserRolesRepository.GetByInstitutionUserId(Id);
                            _dataUnitOfWork.BaseUow.UserRolesRepository.RemoveRange(userRoles);
                            removedUserRoles.Add(userRoles.Select(x => x.Id).ToList());

                            var organizationInstitutionUser = _dataUnitOfWork.BaseUow.OrganizationInstitutionUsersRepository.GetByInstitutionUserId(Id);
                            if (organizationInstitutionUser != null)
                            {
                            _dataUnitOfWork.BaseUow.OrganizationInstitutionUsersRepository.Remove(organizationInstitutionUser, true);
                                removedOrganizationInstitutionUsers.Add(organizationInstitutionUser.Id);
                            }


                            // prodiskutovati ovaj dio...
                            var tempUserId = _dataUnitOfWork.BaseUow.InstitutionUsersRepository.GetById(Id).UserId;

                            _dataUnitOfWork.BaseUow.InstitutionUsersRepository.Remove(_dataUnitOfWork.BaseUow.InstitutionUsersRepository.GetById(Id), true);
                            removedInstitutionUsers.Add(tempUserId);

                            _dataUnitOfWork.BaseUow.UsersRepository.Remove(_dataUnitOfWork.BaseUow.UsersRepository.GetById(tempUserId), true);
                            removedUsers.Add(tempUserId);
                        }
                        _dataUnitOfWork.BaseUow.UsersRepository.SaveChanges();


                        #region Log
                        //
                        foreach (var Id in removedUsers)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.Users, Id, GetControllerName(), GetActionName(), null);
                        }
                        foreach (var Id in removedPhotos)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.PersonPhotos, Id, GetControllerName(), GetActionName(), null);
                        }
                        foreach (var Id in removedPersonDetails)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.PersonDetails, Id, GetControllerName(), GetActionName(), null);
                        }
                        foreach (var Id in removedPersons)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.Persons, Id, GetControllerName(), GetActionName(), null);
                        }
                        foreach (var listContacts in removedContacts)
                        {
                            foreach (var Id in listContacts)
                            {
                                _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.PersonContacts, Id, GetControllerName(), GetActionName(), null);
                            }
                        }

                        //Base
                        foreach (var listLicensePeriods in removedLicensePeriods)
                        {
                            foreach (var Id in listLicensePeriods)
                            {
                                _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.LicensePeriods, Id, GetControllerName(), GetActionName(), null);
                            }
                        }
                        foreach (var listUserRoles in removedUserRoles)
                        {
                            foreach (var Id in listUserRoles)
                            {
                                _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.UserRoles, Id, GetControllerName(), GetActionName(), null);
                            }
                        }
                        foreach (var Id in removedUsers)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.Users, Id, GetControllerName(), GetActionName(), null);
                        }
                        foreach (var Id in removedOrganizationInstitutionUsers)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.OrganizationInstitutionUsers, Id, GetControllerName(), GetActionName(), null);
                        }
                        foreach (var Id in removedInstitutionUsers)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.InstitutionUsers, Id, GetControllerName(), GetActionName(), null);
                        }
                        foreach (var Id in removedLicenses)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.MemberLicenses, Id, GetControllerName(), GetActionName(), null);
                        }
                        foreach (var Id in removedMembers)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.Members, Id, GetControllerName(), GetActionName(), null);
                        }
                        #endregion
                        notification = new Notification(NotificationTypes.Success, Localizer.Removed, string.Format(Localizer.SuccessfullyRemovedName, Localizer.Staff));

                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();

                        notification = new Notification(NotificationTypes.Error, _localizer.ErrorFriendly, _localizer.AnErrorOccurredFriendly);
                        foreach (var Id in list)
                        {
                            _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.Users, Id, GetControllerName(), GetActionName(), ex);
                            _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.Persons, Id, GetControllerName(), GetActionName(), ex);
                        }
                    }
            }
            return Json(notification.ConvertToJson());
        }
        #endregion


        #region Worksheet

        public IActionResult Worksheet(int Id)
        {
            return PartialView("Worksheet");
        }

        #endregion

        #region License
        public JsonResult JsonIndexLicense(int MemberId)
        {
            var licenseDTO = _dataUnitOfWork.BaseUow.LicenseDTORepository.GetByMemberId(MemberId).ToList();
            var licenseDTOJson = JsonConvert.SerializeObject(licenseDTO);
            return new JsonResult(licenseDTOJson);
        }
        public IActionResult AddLicense(int MemberId)
        {
            var model = new LicenseAddViewModel
            {
                License = new MemberLicenseViewModel
                {
                    MemberId = MemberId
                },
                LicenceTypePeriod = "perPeriod",
                LicensePeriod = new LicensePeriodViewModel
                {
                    Active = true,
                }
            };
            return PartialView("_AddLicense", model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddLicense(LicenseAddViewModel model)
        {
            if (model.License.LicenceNumber == null)
                ModelState.AddModelError(nameof(Localizer.ErrorMessageLicenseNumberReq), Localizer.ErrorMessageLicenseNumberReq);
            if (model.LicenceTypePeriod == "perPeriod")
            {
                if (model.LicensePeriod.CreatedDate == null || model.LicensePeriod.EndDate == null)
                {
                    ModelState.AddModelError(nameof(Localizer.ErrorMessageStartDateReq), Localizer.ErrorMessageStartDateReq);
                    ModelState.AddModelError(nameof(Localizer.ErrorMessageEndDateReq), Localizer.ErrorMessageEndDateReq);
                }
                else if (model.LicensePeriod.CreatedDate > model.LicensePeriod.EndDate)
                {
                    ModelState.AddModelError(nameof(Localizer.ErrorMessageCreatedDateBeforeExpiredDate), Localizer.ErrorMessageCreatedDateBeforeExpiredDate);
                }
                if (model.LicensePeriod.LicenseTypeId == 0)
                {
                    ModelState.AddModelError(nameof(Localizer.ErrorMessageTypeOfLiceseReq), Localizer.ErrorMessageTypeOfLiceseReq);
                }
            }
            if (ModelState.IsValid)
            {
                MemberLicense license = null;
                try
                {
                    if (model.LicenceTypePeriod == "permanent")
                        model.License.Permanent = true;
                    license = model.License;
                    license.Active = true;
                    license.CreatedDate = DateTime.Now;
                    _dataUnitOfWork.BaseUow.MemberLicensesRepository.Add(license);
                    _dataUnitOfWork.BaseUow.MemberLicensesRepository.SaveChanges();

                    _dataUnitOfWork.BaseUow.MemberLicensesRepository.SetActiveLicenseByMemberId(license.Id, license.MemberId);
                    _dataUnitOfWork.BaseUow.MemberLicensesRepository.SaveChanges();

                    if (model.LicenceTypePeriod == "perPeriod")
                    {
                        model.LicensePeriod.Active = true;
                        model.LicensePeriod.MemberLicenseId = license.Id;
                        AddLicensePeriod(model.LicensePeriod);
                    }
                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.MemberLicenses, license.Id, GetControllerName(), GetActionName(), null);
                    return Json(new { licenseId = license.Id, licenseNumber = license.LicenceNumber, perPeriod = model.LicenceTypePeriod == "perPeriod" ? true : false, success = true, notification = new Notification(NotificationTypes.Success, Localizer.Saved, string.Format(Localizer.SuccessfullySavedAndRecordName, model.License.LicenceNumber)).ConvertToJson() });

                }
                catch (Exception ex)
                {
                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Add, Tables.Base.MemberLicenses, license.Id, GetControllerName(), GetActionName(), ex);
                    return Json(new { success = true, notification = new Notification(NotificationTypes.Error, Localizer.Error, Localizer.ErrorFriendly).ConvertToJson() });
                }
            }
            return PartialView("_AddLicense", model);
        }
        public IActionResult DeactivateLicense(int Id)
        {
            Notification notification = null;
            MemberLicense license = null;
            try
            {
                license = _dataUnitOfWork.BaseUow.MemberLicensesRepository.GetById(Id);
                license.Active = false;
                _dataUnitOfWork.BaseUow.MemberLicensesRepository.Update(license);
                _dataUnitOfWork.BaseUow.MemberLicensesRepository.SaveChanges();

                _dataUnitOfWork.BaseUow.LicensePeriodsRepository.DeactivateByLicenseId(Id);
                _dataUnitOfWork.BaseUow.LicensePeriodsRepository.SaveChanges();

                _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.MemberLicenses, license.Id, GetControllerName(), GetActionName(), null);
                notification = new Notification(NotificationTypes.Success, Localizer.Deactivated, string.Format(Localizer.SuccessfullyDeactivated, Localizer.License));

            }
            catch (Exception ex)
            {
                _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Edit, Tables.Base.MemberLicenses, license.Id, GetControllerName(), GetActionName(), null);
                notification = new Notification(NotificationTypes.Error, _localizer.ErrorFriendly, _localizer.AnErrorOccurredFriendly);
            }
            return Json(notification.ConvertToJson());
        }

        #region LicensePeriods

        [HttpGet]
        public IActionResult AddLicensePeriod(int licenseId)
        {
            var model = new LicensePeriodViewModel()
            {
                CreatedDate = DateTime.Now,
                Active = true,
                MemberLicenseId = licenseId
            };
            return PartialView("_AddLicensePeriod", model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddLicensePeriod(LicensePeriodViewModel model)
        {
            if (model.CreatedDate == null || model.EndDate == null)
            {
                ModelState.AddModelError(nameof(Localizer.ErrorMessageStartDateReq), Localizer.ErrorMessageStartDateReq);
                ModelState.AddModelError(nameof(Localizer.ErrorMessageEndDateReq), Localizer.ErrorMessageEndDateReq);
            }
            else if (model.CreatedDate > model.EndDate)
            {
                ModelState.AddModelError(nameof(Localizer.ErrorMessageCreatedDateBeforeExpiredDate), Localizer.ErrorMessageCreatedDateBeforeExpiredDate);
            }
            if (model.LicenseTypeId == 0)
            {
                ModelState.AddModelError(nameof(Localizer.ErrorMessageTypeOfLiceseReq), Localizer.ErrorMessageTypeOfLiceseReq);
            }
            if (ModelState.IsValid)
            {
                LicensePeriod licensePeriod = null;
                try
                {
                    licensePeriod = model;
                    licensePeriod.Active = true;
                    _dataUnitOfWork.BaseUow.LicensePeriodsRepository.Add(licensePeriod);
                    _dataUnitOfWork.BaseUow.LicensePeriodsRepository.SaveChanges();

                    _dataUnitOfWork.BaseUow.LicensePeriodsRepository.SetActiveLicesePeriodByLicenseId(licensePeriod.Id, licensePeriod.MemberLicenseId);
                    _dataUnitOfWork.BaseUow.LicensePeriodsRepository.SaveChanges();

                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.LicensePeriods, licensePeriod.Id, GetControllerName(), GetActionName(), null);
                    return Json(new { success = true, notification = new Notification(NotificationTypes.Success, Localizer.Saved, string.Format(Localizer.SuccessfullySavedAndRecordName, string.Empty)).ConvertToJson() });

                }
                catch (Exception ex)
                {
                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Add, Tables.Base.LicensePeriods, licensePeriod.Id, GetControllerName(), GetActionName(), ex);
                    return Json(new { success = true, notification = new Notification(NotificationTypes.Error, Localizer.Error, Localizer.ErrorFriendly).ConvertToJson() });
                }
            }
            return PartialView("_AddLicensePeriod", model);
        }
        public IActionResult EditLicensePeriod(int Id)
        {
            var period = _dataUnitOfWork.BaseUow.LicensePeriodsRepository.GetById(Id);
            var model = new LicensePeriodViewModel();
            model = period;
            return PartialView("_EditLicensePeriod", model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditLicensePeriod(LicensePeriodViewModel model)
        {
            if (model.CreatedDate == null || model.EndDate == null)
            {
                ModelState.AddModelError(nameof(Localizer.ErrorMessageStartDateReq), Localizer.ErrorMessageStartDateReq);
                ModelState.AddModelError(nameof(Localizer.ErrorMessageEndDateReq), Localizer.ErrorMessageEndDateReq);
            }
            else if (model.CreatedDate > model.EndDate)
            {
                ModelState.AddModelError(nameof(Localizer.ErrorMessageCreatedDateBeforeExpiredDate), Localizer.ErrorMessageCreatedDateBeforeExpiredDate);
            }
            if (model.LicenseTypeId == 0)
            {
                ModelState.AddModelError(nameof(Localizer.ErrorMessageTypeOfLiceseReq), Localizer.ErrorMessageTypeOfLiceseReq);
            }
            if (ModelState.IsValid)
            {
                LicensePeriod licensePeriod = null;
                try
                {
                    licensePeriod = model;
                    _dataUnitOfWork.BaseUow.LicensePeriodsRepository.Update(licensePeriod);
                    _dataUnitOfWork.BaseUow.LicensePeriodsRepository.SaveChanges();

                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.LicensePeriods, licensePeriod.Id, GetControllerName(), GetActionName(), null);
                    return Json(new { success = true, notification = new Notification(NotificationTypes.Success, Localizer.Saved, string.Format(Localizer.SuccessfullySavedAndRecordName, string.Empty)).ConvertToJson() });

                }
                catch (Exception ex)
                {
                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Edit, Tables.Base.LicensePeriods, licensePeriod.Id, GetControllerName(), GetActionName(), ex);
                    return Json(new { success = true, notification = new Notification(NotificationTypes.Error, Localizer.Error, Localizer.ErrorFriendly).ConvertToJson() });
                }
            }
            return PartialView("_EditLicensePeriod", model);
        }

        public IActionResult DeleteLicensePeriod(int Id)
        {
            Notification notification = null;
            LicensePeriod licensePeriod = null;
            MemberLicense license = null;
            try
            {
                licensePeriod = _dataUnitOfWork.BaseUow.LicensePeriodsRepository.GetById(Id);
                if (licensePeriod.Active)
                {
                    license = _dataUnitOfWork.BaseUow.MemberLicensesRepository.GetById(licensePeriod.MemberLicenseId);
                    license.Active = false;
                    _dataUnitOfWork.BaseUow.MemberLicensesRepository.Update(license);
                }
                _dataUnitOfWork.BaseUow.LicensePeriodsRepository.Remove(licensePeriod, true);
                _dataUnitOfWork.BaseUow.LicensePeriodsRepository.SaveChanges();

                if (licensePeriod.Active)
                {
                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.MemberLicenses, license.Id, GetControllerName(), GetActionName(), null);
                }
                _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.LicensePeriods, licensePeriod.Id, GetControllerName(), GetActionName(), null);
                notification = new Notification(NotificationTypes.Success, Localizer.Removed, string.Format(Localizer.SuccessfullyRemovedName, Localizer.License));
            }
            catch (Exception ex)
            {
                if (licensePeriod.Active)
                {
                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Edit, Tables.Base.MemberLicenses, license.Id, GetControllerName(), GetActionName(), ex);
                }
                _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.LicensePeriods, licensePeriod.Id, GetControllerName(), GetActionName(), ex);
                notification = new Notification(NotificationTypes.Error, _localizer.ErrorFriendly, _localizer.AnErrorOccurredFriendly);
            }
            return Json(notification.ConvertToJson());
        }
        public IActionResult DeleteAllLicensePeriod(List<int> list)
        {
            Notification notification = null;
            MemberLicense license = null;
            try
            {
                var updatedLicenseId = new int();
                var periods = _dataUnitOfWork.BaseUow.LicensePeriodsRepository.GetAll().ToList().Where(x => list.Contains(x.Id)).ToList();
                foreach (var period in periods)
                {
                    if (period.Active)
                    {
                        license = _dataUnitOfWork.BaseUow.MemberLicensesRepository.GetById(period.MemberLicenseId);
                        license.Active = false;
                        _dataUnitOfWork.BaseUow.MemberLicensesRepository.Update(license);
                        updatedLicenseId = license.Id;
                    }
                }
                _dataUnitOfWork.BaseUow.LicensePeriodsRepository.RemoveRange(periods);
                _dataUnitOfWork.BaseUow.LicensePeriodsRepository.SaveChanges();

                _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.MemberLicenses, updatedLicenseId, GetControllerName(), GetActionName(), null);
                foreach (var Id in list)
                {
                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.LicensePeriods, Id, GetControllerName(), GetActionName(), null);
                }
                notification = new Notification(NotificationTypes.Success, Localizer.Removed, string.Format(Localizer.SuccessfullyRemovedName, Localizer.Licenses));
            }
            catch (Exception ex)
            {
                if (license != null)
                {
                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Edit, Tables.Base.MemberLicenses, license.Id, GetControllerName(), GetActionName(), ex);
                }
                foreach (var Id in list)
                {
                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.LicensePeriods, Id, GetControllerName(), GetActionName(), ex);
                }
                notification = new Notification(NotificationTypes.Error, _localizer.ErrorFriendly, _localizer.AnErrorOccurredFriendly);
            }
            return Json(notification.ConvertToJson());
        }
        #endregion



        #endregion


        #region Helpers
        public IActionResult GenerateUsername(string firstname, string lastname)
        {
            string _username;
            var newUsername = firstname + "." + lastname;
            var numberOfExists = _dataUnitOfWork.BaseUow.PersonsRepository.NuberOfExists(firstname, lastname);
            if (numberOfExists == 0)
            {
                _username = firstname + "." + lastname;
            }
            else
            {
                _username = firstname + "." + lastname + numberOfExists.ToString();
            }
            return Json(new { success = true, username = _username.ToLower() });
        }

        #endregion
    }
}