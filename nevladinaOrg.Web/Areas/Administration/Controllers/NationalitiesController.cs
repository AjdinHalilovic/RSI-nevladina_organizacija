using Core.Constants;
using Core.Entities.Base;
using DAL;
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
    public class NationalitiesController : BaseController
    {
        #region Properties
        private readonly IDataUnitOfWork _dataUnitOfWork;

        private readonly Localizer _localizer;
        private readonly ILogger _logger;
        #endregion
        public NationalitiesController(IDataUnitOfWork dataUnitOfWork,
            Localizer localizer, ILogger logger,
            IBreadcrumb breadcrumb, IStringLocalizerFactory stringLocalizerFactory)
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
            IEnumerable<Nationality> nationalities = _dataUnitOfWork.BaseUow.NationalitiesRepository.GetAll();

            var nationalitiesJson = JsonConvert.SerializeObject(nationalities);

            return new JsonResult(nationalitiesJson);
        }
        #endregion

        #region Add
        public IActionResult Add()
        {
            return PartialView(MagicStrings.ViewNames._Add, new NationalityViewModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Add(NationalityViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView(MagicStrings.ViewNames._Add, model);

            if (_dataUnitOfWork.BaseUow.NationalitiesRepository.GetExists(model.Name))
                ModelState.AddModelError(nameof(_localizer.RecordAlreadyExists), _localizer.RecordAlreadyExists);

            if (ModelState.IsValid)
            {
                try
                {
                    _dataUnitOfWork.BaseUow.NationalitiesRepository.Add(model);
                    _dataUnitOfWork.BaseUow.NationalitiesRepository.SaveChanges();

                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.Nationalities, model.Id, GetControllerName(), GetActionName(), null);

                    return Json(new { success = true, notification = new Notification(NotificationTypes.Success, _localizer.Saved, string.Format(_localizer.SuccessfullySavedAndRecordName, model.Name)).ConvertToJson() });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(_localizer.Error), _localizer.Error);

                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Add, Tables.Base.Nationalities, model.Id, GetControllerName(), GetActionName(), ex);
                }
            }

            return PartialView(MagicStrings.ViewNames._Add, model);
        }
        #endregion

        #region Edit
        public IActionResult Edit(int id)
        {
            NationalityViewModel model = _dataUnitOfWork.BaseUow.NationalitiesRepository.GetById(id);
            return PartialView(MagicStrings.ViewNames._Edit, model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(NationalityViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView(MagicStrings.ViewNames._Edit, model);

            if (_dataUnitOfWork.BaseUow.NationalitiesRepository.GetExists(model.Name))
                ModelState.AddModelError(nameof(_localizer.RecordAlreadyExists), _localizer.RecordAlreadyExists);

            if (ModelState.IsValid)
            {
                try
                {
                    _dataUnitOfWork.BaseUow.NationalitiesRepository.Update(model);
                    _dataUnitOfWork.BaseUow.NationalitiesRepository.SaveChanges();

                    //TempData[MagicStrings.TempDataNames.jsNotifications] = new Notification(NotificationTypes.Success, _localizer.Saved, string.Format(_localizer.SuccessfullySavedAndRecordName, model.Name)).ConvertToJson();

                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.Nationalities, model.Id, GetControllerName(), GetActionName(), null);

                    return Json(new { success = true, notification = new Notification(NotificationTypes.Success, _localizer.Saved, string.Format(_localizer.SuccessfullySavedAndRecordName, model.Name)).ConvertToJson() });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(_localizer.Error), _localizer.Error);

                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Edit, Tables.Base.Nationalities, model.Id, GetControllerName(), GetActionName(), ex);
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
                Nationality model = _dataUnitOfWork.BaseUow.NationalitiesRepository.GetById(Id);

                _dataUnitOfWork.BaseUow.NationalitiesRepository.Remove(model);
                _dataUnitOfWork.BaseUow.NationalitiesRepository.SaveChanges();

                 notification = new Notification(NotificationTypes.Success, _localizer.Removed, string.Format(_localizer.SuccessfullyRemovedName, model.Name));
                _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.Nationalities, model.Id, GetControllerName(), GetActionName(), null);
            }
            catch (Exception ex)
            {
                notification = new Notification(NotificationTypes.Error, Localizer.ErrorFriendly, Localizer.AnErrorOccurredFriendly);
                _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.Nationalities, Id, GetControllerName(), GetActionName(), ex);
            }
            return Json(notification.ConvertToJson());

        }
        public JsonResult DeleteAll(List<int> list)
        {
            List<Nationality> modelList = new List<Nationality>();
            Notification notification = null;
            try
            {
                foreach (var item in list)
                {
                    modelList.Add(_dataUnitOfWork.BaseUow.NationalitiesRepository.GetById(item));
                }
                foreach (var item in modelList)
                {
                    _dataUnitOfWork.BaseUow.NationalitiesRepository.Remove(item);
                }
                _dataUnitOfWork.BaseUow.NationalitiesRepository.SaveChanges();
                foreach (var item in modelList)
                {
                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.Nationalities, item.Id, GetControllerName(), GetActionName(), null);
                }
                notification = new Notification(NotificationTypes.Success, Localizer.Removed, string.Format(Localizer.SuccessfullyRemovedName, Localizer.Nationalities));
            }
            catch (Exception ex)
            {
                notification = new Notification(NotificationTypes.Error, Localizer.ErrorFriendly, Localizer.AnErrorOccurredFriendly);
                foreach (var item in list)
                {
                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.Nationalities, item, GetControllerName(), GetActionName(), ex);
                }
            }
            return Json(notification.ConvertToJson());
        }
        #endregion

    }
}