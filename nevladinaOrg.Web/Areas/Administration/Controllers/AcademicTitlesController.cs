using Core.Constants;
using Core.Entities;
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
    public class AcademicTitlesController : BaseController
    {
        #region Properties
        private readonly IDataUnitOfWork _dataUnitOfWork;

        private ILogger _logger;
        #endregion

        public AcademicTitlesController(
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
        public JsonResult JsonIndex()
        {
            IEnumerable<AcademicTitle> academicTitles = _dataUnitOfWork.BaseUow.AcademicTitlesRepository.GetAll();

            var academicTitlesJson = JsonConvert.SerializeObject(academicTitles);

            return new JsonResult(academicTitlesJson);
        }
        #endregion

        #region Add
        [HttpGet]
        public IActionResult Add()
        {
            return PartialView(MagicStrings.ViewNames._Add, new AcademicTitleViewModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Add(AcademicTitleViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView(MagicStrings.ViewNames._Add, model);

            if (_dataUnitOfWork.BaseUow.AcademicTitlesRepository.GetExists(model.Name))
                ModelState.AddModelError(nameof(Localizer.RecordAlreadyExists), Localizer.RecordAlreadyExists);

            if (ModelState.IsValid)
            {
                try
                {
                    _dataUnitOfWork.BaseUow.AcademicTitlesRepository.Add(model);
                    _dataUnitOfWork.BaseUow.AcademicTitlesRepository.SaveChanges();

                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.AcademicTitles, model.Id, GetControllerName(), GetActionName(), null);

                    return Json(new { success = true, notification = new Notification(NotificationTypes.Success, Localizer.Saved, string.Format(Localizer.SuccessfullySavedAndRecordName, model.Name)).ConvertToJson() });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(Localizer.Error), Localizer.Error);

                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Add, Tables.Base.AcademicTitles, model.Id, GetControllerName(), GetActionName(), ex);
                }
            }

            return PartialView(MagicStrings.ViewNames._Add, model);
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int AcademicTitleId)
        {
            AcademicTitleViewModel academictitleViewModel = _dataUnitOfWork.BaseUow.AcademicTitlesRepository.GetById(AcademicTitleId);
            return PartialView(MagicStrings.ViewNames._Edit, academictitleViewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(AcademicTitleViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView(MagicStrings.ViewNames._Edit, model);

            try
            {
                _dataUnitOfWork.BaseUow.AcademicTitlesRepository.Update(model);
                _dataUnitOfWork.BaseUow.AcademicTitlesRepository.SaveChanges();

                _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.AcademicTitles, model.Id, GetControllerName(), GetActionName(), null);

                return Json(new { success = true, notification = new Notification(NotificationTypes.Success, Localizer.Saved, string.Format(Localizer.SuccessfullySavedAndRecordName, model.Name)).ConvertToJson() });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(Localizer.Error), Localizer.Error);

                _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Edit, Tables.Base.AcademicTitles, model.Id, GetControllerName(), GetActionName(), ex);
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
                AcademicTitle academicTitle = _dataUnitOfWork.BaseUow.AcademicTitlesRepository.GetById(Id);
                if (academicTitle != null)
                {
                    _dataUnitOfWork.BaseUow.AcademicTitlesRepository.Remove(academicTitle);
                    _dataUnitOfWork.BaseUow.AcademicTitlesRepository.SaveChanges();
                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.AcademicDegrees, academicTitle.Id, GetControllerName(), GetActionName(), null);
                    notification = new Notification(NotificationTypes.Success, Localizer.Saved, string.Format(Localizer.SuccessfullySavedAndRecordName, academicTitle.Name));
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
            List<AcademicTitle> modelList = new List<AcademicTitle>();
            Notification notification = null;
            try
            {
                foreach (var item in list)
                {
                    modelList.Add(_dataUnitOfWork.BaseUow.AcademicTitlesRepository.GetById(item));
                }
                foreach (var item in modelList)
                {
                    _dataUnitOfWork.BaseUow.AcademicTitlesRepository.Remove(item);
                }
                _dataUnitOfWork.BaseUow.AcademicTitlesRepository.SaveChanges();
                foreach (var item in modelList)
                {
                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.AcademicTitles, item.Id, GetControllerName(), GetActionName(), null);
                }
                notification = new Notification(NotificationTypes.Success, Localizer.Removed, string.Format(Localizer.SuccessfullyRemovedName, Localizer.AcademicTitles));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(Localizer.Error), Localizer.Error);

                notification = new Notification(NotificationTypes.Error, Localizer.ErrorFriendly, Localizer.AnErrorOccurredFriendly);
                foreach (var item in list)
                {
                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.AcademicTitles, item, GetControllerName(), GetActionName(), ex);
                }
            }
            return Json(notification.ConvertToJson());

        }
        #endregion
    }
}