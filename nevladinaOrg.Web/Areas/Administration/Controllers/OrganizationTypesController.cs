using Core.Constants;
using Core.Entities.Base;
using DAL;
using nevladinaOrg.Web.Areas.Administration.ViewModels;
using nevladinaOrg.Web.Constants;
using nevladinaOrg.Web.Controllers;
using nevladinaOrg.Web.Helpers;
using nevladinaOrg.Web.Helpers.Breadcrumb;
using nevladinaOrg.Web.Helpers.Logger;
using nevladinaOrg.Web.Helpers.SelectListHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace nevladinaOrg.Web.Areas.Administration.Controllers
{
    [Area(MagicStrings.AreaNames.Administration)]
    [Authorization]
    public class OrganizationTypesController : BaseController
    {
        #region Properties
        private IDataUnitOfWork _dataUnitOfWork;

        private ISelectListHelper _selectList;
        private ILogger _logger;
        #endregion
        public OrganizationTypesController(
            IDataUnitOfWork dataUnitOfWork,
            IBreadcrumb breadcrumb,
            IStringLocalizerFactory stringLocalizerFactory,
            ILogger logger,
            ISelectListHelper selectList)
            : base(breadcrumb, stringLocalizerFactory)
        {
            _dataUnitOfWork = dataUnitOfWork;
            _logger = logger;
            _selectList = selectList;
        }

        #region Index
        public IActionResult JsonIndex()
        {
            IEnumerable<OrganizationType> organizationType = _dataUnitOfWork.BaseUow.OrganizationTypesRepository.GetAll();

            var organizationTypeJson = JsonConvert.SerializeObject(organizationType);

            return new JsonResult(organizationTypeJson);
        }
        #endregion

        #region Add
        [HttpGet]
        public IActionResult Add()
        {
            return PartialView(MagicStrings.ViewNames._Add, new OrganizationTypeViewModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Add(OrganizationTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView(MagicStrings.ViewNames._Add, model);

            if (_dataUnitOfWork.BaseUow.OrganizationTypesRepository.GetExists(model.Name))
                ModelState.AddModelError(nameof(Localizer.RecordAlreadyExists), Localizer.RecordAlreadyExists);

            if (ModelState.IsValid)
            {
                try
                {
                    _dataUnitOfWork.BaseUow.OrganizationTypesRepository.Add(model);
                    _dataUnitOfWork.BaseUow.OrganizationTypesRepository.SaveChanges();

                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.OrganizationTypes, model.Id, GetControllerName(), GetActionName(), null);

                    return Json(new { success = true, notification = new Notification(NotificationTypes.Success, Localizer.Saved, string.Format(Localizer.SuccessfullySavedAndRecordName, model.Name)).ConvertToJson() });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(Localizer.Error), Localizer.Error);

                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Add, Tables.Base.OrganizationTypes, model.Id, GetControllerName(), GetActionName(), ex);
                }
            }

            return PartialView(MagicStrings.ViewNames._Add, model);
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int OrganizationTypeId)
        {
            OrganizationTypeViewModel organizationTypeViewModel = _dataUnitOfWork.BaseUow.OrganizationTypesRepository.GetById(OrganizationTypeId);
            return PartialView(MagicStrings.ViewNames._Edit, organizationTypeViewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(OrganizationTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView(MagicStrings.ViewNames._Edit, model);

            try
            {
                _dataUnitOfWork.BaseUow.OrganizationTypesRepository.Update(model);
                _dataUnitOfWork.BaseUow.OrganizationTypesRepository.SaveChanges();

                _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.OrganizationTypes, model.Id, GetControllerName(), GetActionName(), null);

                return Json(new { success = true, notification = new Notification(NotificationTypes.Success, Localizer.Saved, string.Format(Localizer.SuccessfullySavedAndRecordName, model.Name)).ConvertToJson() });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(Localizer.Error), Localizer.Error);

                _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Edit, Tables.Base.OrganizationTypes, model.Id, GetControllerName(), GetActionName(), ex);
            }
            return PartialView(MagicStrings.ViewNames._Edit, model);
        }
        #endregion

        #region Delete
        public JsonResult Delete(int Id)
        {
            Notification notification = null;
            try
            {
                OrganizationType organizationType = _dataUnitOfWork.BaseUow.OrganizationTypesRepository.GetById(Id);
                if (organizationType != null)
                {
                    _dataUnitOfWork.BaseUow.OrganizationTypesRepository.Remove(organizationType);
                    _dataUnitOfWork.BaseUow.OrganizationTypesRepository.SaveChanges();
                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.AcademicDegrees, organizationType.Id, GetControllerName(), GetActionName(), null);
                    notification = new Notification(NotificationTypes.Success, Localizer.Removed, string.Format(Localizer.SuccessfullyRemovedName, organizationType.Name));
                }
            }
            catch (Exception ex)
            {
                notification = new Notification(NotificationTypes.Error, Localizer.ErrorFriendly, Localizer.AnErrorOccurredFriendly);
                _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.AcademicDegrees, Id, GetControllerName(), GetActionName(), ex);
            }
            return Json(notification.ConvertToJson());
        }
        public JsonResult DeleteAll(List<int> list)
        {
            List<OrganizationType> modelList = new List<OrganizationType>();
            Notification notification = null;
            try
            {
                foreach (var item in list)
                {
                    modelList.Add(_dataUnitOfWork.BaseUow.OrganizationTypesRepository.GetById(item));
                }
                foreach (var item in modelList)
                {
                    _dataUnitOfWork.BaseUow.OrganizationTypesRepository.Remove(item);
                }
                _dataUnitOfWork.BaseUow.OrganizationTypesRepository.SaveChanges();
                foreach (var item in modelList)
                {
                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.OrganizationTypes, item.Id, GetControllerName(), GetActionName(), null);
                }
                notification = new Notification(NotificationTypes.Success, Localizer.Removed, string.Format(Localizer.SuccessfullyRemovedName, Localizer.OrganizationTypes));
            }
            catch (Exception ex)
            {
                notification = new Notification(NotificationTypes.Error, Localizer.ErrorFriendly, Localizer.AnErrorOccurredFriendly);
                foreach (var item in list)
                {
                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.OrganizationTypes, item, GetControllerName(), GetActionName(), ex);
                }
            }
            return Json(notification.ConvertToJson());

        }
        #endregion
    }
}
