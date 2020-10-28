using Core.Constants;
using Core.Entities.Base;
using DAL;
using DAL.Repositories.Base.IRepository;
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
    public class MaritalStatusesController : BaseController
    {
        #region Properties
        private readonly IDataUnitOfWork _dataUnitOfWork;

        private ISelectListHelper _selectList;
        private ILogger _logger;
        #endregion

        public MaritalStatusesController(
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
            IEnumerable<MaritalStatus> maritalStatuses = _dataUnitOfWork.BaseUow.MartialStatusesRepository.GetAll();

            var maritalStatusesJson = JsonConvert.SerializeObject(maritalStatuses);

            return new JsonResult(maritalStatusesJson);
        }
        #endregion

        #region Add
        [HttpGet]
        public IActionResult Add()
        {
            return PartialView(MagicStrings.ViewNames._Add, new MaritalStatusViewModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Add(MaritalStatusViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView(MagicStrings.ViewNames._Add, model);

            if (_dataUnitOfWork.BaseUow.MartialStatusesRepository.GetExists(model.Name))
                ModelState.AddModelError(nameof(Localizer.RecordAlreadyExists), Localizer.RecordAlreadyExists);

            if (ModelState.IsValid)
            {
                try
                {
                    _dataUnitOfWork.BaseUow.MartialStatusesRepository.Add(model);
                    _dataUnitOfWork.BaseUow.MartialStatusesRepository.SaveChanges();

                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.MaritalStatuses, model.Id, GetControllerName(), GetActionName(), null);

                    return Json(new { success = true, notification = new Notification(NotificationTypes.Success, Localizer.Saved, string.Format(Localizer.SuccessfullySavedAndRecordName, model.Name)).ConvertToJson() });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(Localizer.Error), Localizer.Error);

                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Add, Tables.Base.MaritalStatuses, model.Id, GetControllerName(), GetActionName(), ex);
                }
            }

            return PartialView(MagicStrings.ViewNames._Add, model);
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int MaritalStatusId)
        {
            MaritalStatusViewModel maritalStatusVM = _dataUnitOfWork.BaseUow.MartialStatusesRepository.GetById(MaritalStatusId);
            return PartialView(MagicStrings.ViewNames._Edit, maritalStatusVM);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(MaritalStatusViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView(MagicStrings.ViewNames._Edit, model);

            try
            {
                _dataUnitOfWork.BaseUow.MartialStatusesRepository.Update(model);
                _dataUnitOfWork.BaseUow.MartialStatusesRepository.SaveChanges();

                _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.MaritalStatuses, model.Id, GetControllerName(), GetActionName(), null);

                return Json(new { success = true, notification = new Notification(NotificationTypes.Success, Localizer.Saved, string.Format(Localizer.SuccessfullySavedAndRecordName, model.Name)).ConvertToJson() });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(Localizer.Error), Localizer.Error);

                _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Edit, Tables.Base.MaritalStatuses, model.Id, GetControllerName(), GetActionName(), ex);
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
                MaritalStatus maritalStatus = _dataUnitOfWork.BaseUow.MartialStatusesRepository.GetById(Id);
                if (maritalStatus != null)
                {
                    _dataUnitOfWork.BaseUow.MartialStatusesRepository.Remove(maritalStatus);
                    _dataUnitOfWork.BaseUow.MartialStatusesRepository.SaveChanges();
                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.MaritalStatuses, Id, GetControllerName(), GetActionName(), null);
                    notification = new Notification(NotificationTypes.Success, Localizer.Removed, string.Format(Localizer.SuccessfullyRemovedName, maritalStatus.Name));
                }
            }
            catch (Exception e)
            {
                notification = new Notification(NotificationTypes.Error, Localizer.Error, Localizer.AnErrorOccurredFriendly);
                _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.MaritalStatuses, Id, GetControllerName(), GetActionName(),e);
            }
            return Json(notification.ConvertToJson());
        }

        public JsonResult DeleteAll(List<int> list)
        {
            List<MaritalStatus> modelList = new List<MaritalStatus>();
            Notification notification = null;
            try
            {
                foreach (var item in list)
                {
                    modelList.Add(_dataUnitOfWork.BaseUow.MartialStatusesRepository.GetById(item));
                }
                _dataUnitOfWork.BaseUow.MartialStatusesRepository.RemoveRange(modelList);
                _dataUnitOfWork.BaseUow.MartialStatusesRepository.SaveChanges();
                foreach (var item in modelList)
                {
                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.MaritalStatuses, item.Id, GetControllerName(), GetActionName(), null);
                }
                notification = new Notification(NotificationTypes.Success, Localizer.Removed, string.Format(Localizer.SuccessfullyRemovedName, Localizer.MaritalStatuses));
            }
            catch (Exception ex)
            {
                notification = new Notification(NotificationTypes.Error, Localizer.ErrorFriendly, Localizer.AnErrorOccurredFriendly);
                foreach (var item in list)
                {
                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.MaritalStatuses, item, GetControllerName(), GetActionName(), ex);
                }
            }
            return Json(notification.ConvertToJson());
        }
        #endregion
    }
}