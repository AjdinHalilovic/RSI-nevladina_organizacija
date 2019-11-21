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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
namespace nevladinaOrg.Web.Areas.Administration.Controllers
{
    [Area(MagicStrings.AreaNames.Administration)]
    [Authorization]
    public class InstitutionTypesController : BaseController
    {
        #region Properties
        private readonly IDataUnitOfWork _dataUnitOfWork;
        private Localizer _localizer;
        private ILogger _logger;
        #endregion
        public InstitutionTypesController(
            IDataUnitOfWork dataUnitOfWork,
            Localizer localizer, ILogger logger,
            IBreadcrumb breadcrumb,
            IStringLocalizerFactory stringLocalizerFactory)
            : base(breadcrumb, stringLocalizerFactory)
        {
            _dataUnitOfWork = dataUnitOfWork;
            _localizer = localizer;
            _logger = logger;
        }
        #region Index
        [HttpGet]
        public JsonResult JsonIndex()
        {
            IEnumerable<InstitutionType> intitutionTypes = _dataUnitOfWork.BaseUow.InstitutionTypesRepository.GetAll();
            var institutionTypesJson = JsonConvert.SerializeObject(intitutionTypes);
            return new JsonResult(institutionTypesJson);
        }
        #endregion
        #region Add
        public IActionResult Add()
        {
            return PartialView(MagicStrings.ViewNames._Add, new InstitutionTypeViewModel());
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Add(InstitutionTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView(MagicStrings.ViewNames._Add, model);
            if (_dataUnitOfWork.BaseUow.InstitutionTypesRepository.GetExists(model.Name))
                ModelState.AddModelError(nameof(_localizer.RecordAlreadyExists), _localizer.RecordAlreadyExists);
            if (ModelState.IsValid)
            {
                try
                {
                    _dataUnitOfWork.BaseUow.InstitutionTypesRepository.Add(model);
                    _dataUnitOfWork.BaseUow.InstitutionTypesRepository.SaveChanges();
                    //TempData[MagicStrings.TempDataNames.jsNotifications] = new Notification(NotificationTypes.Success, _localizer.Saved, string.Format(_localizer.SuccessfullySavedAndRecordName, model.Name)).ConvertToJson();
                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.InstitutionTypes, model.Id, GetControllerName(), GetActionName(), null);
                    return Json(new { success = true, notification = new Notification(NotificationTypes.Success, _localizer.Saved, string.Format(_localizer.SuccessfullySavedAndRecordName, model.Name)).ConvertToJson() });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(_localizer.Error), _localizer.Error);
                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Add, Tables.Base.InstitutionTypes, model.Id, GetControllerName(), GetActionName(), ex);
                }
            }
            return PartialView(MagicStrings.ViewNames._Add, model);
        }
        #endregion
        #region Edit
        public IActionResult Edit(int id)
        {
            InstitutionTypeViewModel model = _dataUnitOfWork.BaseUow.InstitutionTypesRepository.GetById(id);
            return PartialView(MagicStrings.ViewNames._Edit, model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(InstitutionTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView(MagicStrings.ViewNames._Edit, model);
            if (_dataUnitOfWork.BaseUow.InstitutionTypesRepository.GetExists(model.Name))
                ModelState.AddModelError(nameof(_localizer.RecordAlreadyExists), _localizer.RecordAlreadyExists);
            if (ModelState.IsValid)
            {
                try
                {
                    _dataUnitOfWork.BaseUow.InstitutionTypesRepository.Update(model);
                    _dataUnitOfWork.BaseUow.InstitutionTypesRepository.SaveChanges();
                    //TempData[MagicStrings.TempDataNames.jsNotifications] = new Notification(NotificationTypes.Success, _localizer.Saved, string.Format(_localizer.SuccessfullySavedAndRecordName, model.Name)).ConvertToJson();
                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.InstitutionTypes, model.Id, GetControllerName(), GetActionName(), null);
                    return Json(new { success = true, notification = new Notification(NotificationTypes.Success, _localizer.Saved, string.Format(_localizer.SuccessfullySavedAndRecordName, model.Name)).ConvertToJson() });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(_localizer.Error), _localizer.Error);
                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Edit, Tables.Base.InstitutionTypes, model.Id, GetControllerName(), GetActionName(), ex);
                }
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
                InstitutionType model = _dataUnitOfWork.BaseUow.InstitutionTypesRepository.GetById(Id);
                _dataUnitOfWork.BaseUow.InstitutionTypesRepository.Remove(model);
                _dataUnitOfWork.BaseUow.InstitutionTypesRepository.SaveChanges();
                notification = new Notification(NotificationTypes.Success, _localizer.Saved, string.Format(_localizer.SuccessfullyRemovedName, model.Name));
                _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.InstitutionTypes, model.Id, GetControllerName(), GetActionName(), null);
            }
            catch (Exception ex)
            {
                notification = new Notification(NotificationTypes.Error, Localizer.ErrorFriendly, Localizer.AnErrorOccurredFriendly);
                _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.InstitutionTypes, Id, GetControllerName(), GetActionName(), ex);
            }
            return Json(notification.ConvertToJson());
        }
        public JsonResult DeleteAll(List<int> list)
        {
            List<InstitutionType> modelList = new List<InstitutionType>();
            Notification notification = null;
            try
            {
                foreach (var item in list)
                {
                    modelList.Add(_dataUnitOfWork.BaseUow.InstitutionTypesRepository.GetById(item));
                }
                foreach (var item in modelList)
                {
                    _dataUnitOfWork.BaseUow.InstitutionTypesRepository.Remove(item);
                }
                _dataUnitOfWork.BaseUow.InstitutionTypesRepository.SaveChanges();
                foreach (var item in modelList)
                {
                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.InstitutionTypes, item.Id, GetControllerName(), GetActionName(), null);
                }
                notification = new Notification(NotificationTypes.Success, Localizer.Removed, string.Format(Localizer.SuccessfullyRemovedName, Localizer.InstitutionTypes));
            }
            catch (Exception ex)
            {
                notification = new Notification(NotificationTypes.Error, Localizer.ErrorFriendly, Localizer.AnErrorOccurredFriendly);
                foreach (var item in list)
                {
                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.InstitutionTypes, item, GetControllerName(), GetActionName(), ex);
                }
            }
            return Json(notification.ConvertToJson());
        }
        #endregion 
    }
}