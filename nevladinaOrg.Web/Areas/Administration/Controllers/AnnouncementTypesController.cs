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
    public class AnnouncementTypesController : BaseController
    {
        #region Properties
        private IDataUnitOfWork _dataUnitOfWork;

        private ILogger _logger;
        #endregion

        public AnnouncementTypesController(
            IDataUnitOfWork dataUnitOfWork,
            IBreadcrumb breadcrumb,
            IStringLocalizerFactory stringLocalizerFactory,
            ILogger logger)
            : base(breadcrumb, stringLocalizerFactory)
        {
            _dataUnitOfWork = dataUnitOfWork;
            _logger = logger;
        }

        #region Index
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region Add
        [HttpGet]
        public IActionResult Add(Enumerations.SaveAndOptions option)
        {
            return PartialView(MagicStrings.ViewNames._Add, new AnnouncementTypeViewModel() { SaveAndOptions = option });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Add(AnnouncementTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView(MagicStrings.ViewNames._Add, model);

            if (_dataUnitOfWork.BaseUow.AnnouncementTypesRepository.GetExists(model.Name))
                ModelState.AddModelError(nameof(Localizer.RecordAlreadyExists), Localizer.RecordAlreadyExists);

            if (ModelState.IsValid)
            {
                try
                {
                    _dataUnitOfWork.BaseUow.AnnouncementTypesRepository.Add(model);
                    _dataUnitOfWork.BaseUow.AnnouncementTypesRepository.SaveChanges();

                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.AnnouncementType, model.Id, GetControllerName(), GetActionName(), null);

                    return Json(new { success = true, notification = new Notification(NotificationTypes.Success, Localizer.Saved, string.Format(Localizer.SuccessfullySavedAndRecordName, model.Name)).ConvertToJson() });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(Localizer.Error), Localizer.Error);

                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Add, Tables.Base.AnnouncementType, model.Id, GetControllerName(), GetActionName(), ex);
                }
            }

            return PartialView(MagicStrings.ViewNames._Add, model);
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int AnnouncementTypeId)
        {
            AnnouncementTypeViewModel academictitleVM = _dataUnitOfWork.BaseUow.AnnouncementTypesRepository.GetById(AnnouncementTypeId);
            return PartialView(MagicStrings.ViewNames._Edit, academictitleVM);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(AnnouncementTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView(MagicStrings.ViewNames._Edit, model);

            if (ModelState.IsValid)
            {
                try
                {
                    _dataUnitOfWork.BaseUow.AnnouncementTypesRepository.Update(model);
                    _dataUnitOfWork.BaseUow.AnnouncementTypesRepository.SaveChanges();

                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.AnnouncementType, model.Id, GetControllerName(), GetActionName(), null);

                    return Json(new { success = true, notification = new Notification(NotificationTypes.Success, Localizer.Saved, string.Format(Localizer.SuccessfullySavedAndRecordName, model.Name)).ConvertToJson() });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(Localizer.Error), Localizer.Error);

                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Edit, Tables.Base.AnnouncementType, model.Id, GetControllerName(), GetActionName(), ex);
                }
            }

            return PartialView(MagicStrings.ViewNames._Edit, model);
        }
        #endregion

        #region Delete
        public JsonResult Delete(int Id)
        {
            Notification notification = null;
            AnnouncementType announcementType = _dataUnitOfWork.BaseUow.AnnouncementTypesRepository.GetById(Id);
            try
            {
                if (announcementType != null)
                {
                    _dataUnitOfWork.BaseUow.AnnouncementTypesRepository.Remove(announcementType);
                    _dataUnitOfWork.BaseUow.AnnouncementTypesRepository.SaveChanges();
                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.AcademicDegrees, Id, GetControllerName(), GetActionName(), null);
                    notification = new Notification(NotificationTypes.Success, Localizer.Removed, string.Format(Localizer.SuccessfullyRemovedName, announcementType.Name));
                }
            }
            catch (Exception ex)
            {
                _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.AcademicDegrees, Id, GetControllerName(), GetActionName(), ex);
                notification = new Notification(NotificationTypes.Error, Localizer.Error, Localizer.AnErrorOccurredFriendly);
            }
            return Json(notification.ConvertToJson() );
        }

        public JsonResult DeleteAll(List<int> list)
        {
            List<AnnouncementType> modelList = new List<AnnouncementType>();
            Notification notification = null;
            try
            {
                foreach (var item in list)
                {
                    modelList.Add(_dataUnitOfWork.BaseUow.AnnouncementTypesRepository.GetById(item));
                }
                _dataUnitOfWork.BaseUow.AnnouncementTypesRepository.RemoveRange(modelList);
                _dataUnitOfWork.BaseUow.AnnouncementTypesRepository.SaveChanges();
                foreach (var item in modelList)
                {
                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.AnnouncementType, item.Id, GetControllerName(), GetActionName(), null);
                }
                notification = new Notification(NotificationTypes.Success, Localizer.Removed, string.Format(Localizer.SuccessfullyRemovedName, Localizer.AnnouncementTypes));
            }
            catch (Exception ex)
            {
                notification = new Notification(NotificationTypes.Error, Localizer.ErrorFriendly, Localizer.AnErrorOccurredFriendly);
                foreach (var item in list)
                {
                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.AnnouncementType, item, GetControllerName(), GetActionName(), ex);
                }
            }
            return Json(notification.ConvertToJson());
        }
        #endregion

        #region Helpers
        public JsonResult GetAnnouncementTypes()
        {
            IEnumerable<AnnouncementType> announcementTypes = _dataUnitOfWork.BaseUow.AnnouncementTypesRepository.GetAll();

            var announcementTypesJson = JsonConvert.SerializeObject(announcementTypes);

            return new JsonResult(announcementTypesJson);
        }
        #endregion
    }
}