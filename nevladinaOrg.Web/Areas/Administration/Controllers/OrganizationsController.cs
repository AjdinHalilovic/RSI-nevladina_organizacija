using Core.Constants;
using Core.Entities.Base;
using Core.Entities.Base.DTO;
using DAL;
using DAL.Repositories.Base.IRepository;
using nevladinaOrg.Web.Areas.Administration.ViewModels;
using nevladinaOrg.Web.Constants;
using nevladinaOrg.Web.Controllers;
using nevladinaOrg.Web.Helpers;
using nevladinaOrg.Web.Helpers.Breadcrumb;
using nevladinaOrg.Web.Helpers.ImageHelper;
using nevladinaOrg.Web.Helpers.Logger;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace nevladinaOrg.Web.Areas.Administration.Controllers
{
    [Area(MagicStrings.AreaNames.Administration)]
    [Authorization]
    public class OrganizationsController : BaseController
    {
        #region Properties
        private readonly IDataUnitOfWork _dataUnitOfWork;

        private ILogger _logger;
        private IHostingEnvironment _hostingEnvironment;
        private IImageHelper _imageHelper;
        #endregion
        public OrganizationsController(
            IDataUnitOfWork dataUnitOfWork,
            IBreadcrumb breadcrumb,
            IStringLocalizerFactory stringLocalizerFactory,
            ILogger logger,
            IHostingEnvironment hostingEnvironment,
            IImageHelper imageHelper
            )
            : base(breadcrumb, stringLocalizerFactory)
        {
            _dataUnitOfWork = dataUnitOfWork;

            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _imageHelper = imageHelper;
        }

        #region Index
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region Add
        [HttpGet]
        public IActionResult Add()
        {
            var model = new OrganizationViewModel { Active = true };
            return PartialView(MagicStrings.ViewNames._Add, model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Add(OrganizationViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView(MagicStrings.ViewNames._Add, model);

            if (_dataUnitOfWork.BaseUow.OrganizationRepository.GetExists(model.Name, model.CountryId, model.CityId, model.OrganizationTypeId))
                ModelState.AddModelError(nameof(Localizer.RecordAlreadyExists), Localizer.RecordAlreadyExists);

            if (ModelState.IsValid)
            {
                using (IDbContextTransaction contextTransaction = _dataUnitOfWork.BaseUow.OrganizationRepository.BeginTransaction())
                {
                        try
                        {
                            if (model.ParentId == 0)
                                model.ParentId = null;
                            Organization organization = model;
                            organization.InstitutionId = LoggedUserIds.InstitutionId;
                            _dataUnitOfWork.BaseUow.OrganizationRepository.Add(organization);
                            _dataUnitOfWork.BaseUow.OrganizationRepository.SaveChanges();

                            contextTransaction.Commit();
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.Organizations, organization.Id, GetControllerName(), GetActionName(), null);

                            return Json(new { success = true, notification = new Notification(NotificationTypes.Success, Localizer.Saved, string.Format(Localizer.SuccessfullySavedAndRecordName, organization.Name)).ConvertToJson() });
                        }
                        catch (Exception ex)
                        {
                            contextTransaction.Rollback();
                            ModelState.AddModelError(nameof(Localizer.Error), Localizer.Error);

                            _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Add, Tables.Base.Organizations, model.Id, GetControllerName(), GetActionName(), ex);
                        }
                }
            }

            return PartialView(MagicStrings.ViewNames._Add, model);
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int OrganizationId)
        {
            OrganizationViewModel model = _dataUnitOfWork.BaseUow.OrganizationRepository.GetById(OrganizationId);
            return PartialView(MagicStrings.ViewNames._Edit, model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(OrganizationViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView(MagicStrings.ViewNames._Edit, model);

            if (ModelState.IsValid)
            {
                using (IDbContextTransaction contextTransaction = _dataUnitOfWork.BaseUow.OrganizationRepository.BeginTransaction())
                {
                        try
                        {
                            if (model.ParentId == 0)
                                model.ParentId = null;
                            Organization organization = model;
                            organization.InstitutionId = LoggedUserIds.InstitutionId;

                            _dataUnitOfWork.BaseUow.OrganizationRepository.Update(organization);
                            _dataUnitOfWork.BaseUow.OrganizationRepository.SaveChanges();

                            contextTransaction.Commit();
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.Organizations, organization.Id, GetControllerName(), GetActionName(), null);

                            return Json(new { success = true, notification = new Notification(NotificationTypes.Success, Localizer.Saved, string.Format(Localizer.SuccessfullySavedAndRecordName, model.Name)).ConvertToJson() });
                        }
                        catch (Exception ex)
                        {
                            contextTransaction.Rollback();
                            ModelState.AddModelError(nameof(Localizer.Error), Localizer.Error);

                            _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Edit, Tables.Base.Organizations, model.Id, GetControllerName(), GetActionName(), ex);
                        }
                }
            }

            return PartialView(MagicStrings.ViewNames._Edit, model);
        }
        #endregion

        #region Delete
        public JsonResult Delete(int Id)
        {
            Notification notification = null;
            using (IDbContextTransaction contextTransaction = _dataUnitOfWork.BaseUow.OrganizationRepository.BeginTransaction())
            {
                    try
                    {
                        Organization organization = _dataUnitOfWork.BaseUow.OrganizationRepository.GetById(Id);
                        _dataUnitOfWork.BaseUow.OrganizationRepository.Remove(organization);
                        _dataUnitOfWork.BaseUow.OrganizationRepository.SaveChanges();

                        contextTransaction.Commit();
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.Organizations, Id, GetControllerName(), GetActionName(), null);

                        notification = new Notification(NotificationTypes.Success, Localizer.Removed, string.Format(Localizer.SuccessfullyRemovedName, organization.Name));
                    }
                    catch (Exception ex)
                    {
                        contextTransaction.Rollback();
                        notification = new Notification(NotificationTypes.Error, Localizer.ErrorFriendly, Localizer.AnErrorOccurredFriendly);

                        _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.Organizations, Id, GetControllerName(), GetActionName(), ex);
                    }
            }
            return Json(notification.ConvertToJson());
        }

        public JsonResult DeleteAll(List<int> list)
        {
            var modelList = new List<Organization>();

            Notification notification = null;
            using (IDbContextTransaction contextTransaction = _dataUnitOfWork.BaseUow.OrganizationRepository.BeginTransaction())
            {
                    try
                    {
                        foreach (var item in list)
                        {
                            modelList.Add(_dataUnitOfWork.BaseUow.OrganizationRepository.GetById(item));
                        }
                        foreach (var item in modelList)
                        {
                            _dataUnitOfWork.BaseUow.OrganizationRepository.Remove(item);
                        }
                        _dataUnitOfWork.BaseUow.OrganizationRepository.SaveChanges();

                        contextTransaction.Commit();
                        foreach (var item in modelList)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.Organizations, item.Id, GetControllerName(), GetActionName(), null);
                        }
                        return Json(new { success = true, notification = new Notification(NotificationTypes.Success, Localizer.Removed, string.Format(Localizer.SuccessfullyRemovedName, Localizer.Organizations)).ConvertToJson() });
                    }
                    catch (Exception ex)
                    {
                        contextTransaction.Rollback();
                        notification = new Notification(NotificationTypes.Error, Localizer.ErrorFriendly, Localizer.AnErrorOccurredFriendly);

                        foreach (var item in list)
                        {
                            _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.Institutions, item, GetControllerName(), GetActionName(), ex);
                        }
                }
            }
            return Json(notification.ConvertToJson());

        }
        #endregion

        #region Helpers
        public JsonResult GetOrganizations()
        {
            IEnumerable<OrganizationDTO> organizations = _dataUnitOfWork.BaseUow.OrganizationsDTORepository.GetAll();

            var organizationsJson = JsonConvert.SerializeObject(organizations);

            return new JsonResult(organizationsJson);
        }

        public JsonResult GetOrganizationsByParentId(int OrganizationParentId)
        {
            IEnumerable<OrganizationDTO> organizations = _dataUnitOfWork.BaseUow.OrganizationsDTORepository.GetByParentId(OrganizationParentId);

            var organizationsJson = JsonConvert.SerializeObject(organizations);

            return new JsonResult(organizationsJson);
        }

        #endregion

    }
}