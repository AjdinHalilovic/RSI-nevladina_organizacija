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
    public class AcademicDegreesController : BaseController
    {
        #region Properties
        private readonly IDataUnitOfWork _dataUnitOfWork;

        private ILogger _logger;
        #endregion

        public AcademicDegreesController(
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
            IEnumerable<AcademicDegree> academicDegrees = _dataUnitOfWork.BaseUow.AcademicDegreesRepository.GetAll();

            var academicDegreesJson = JsonConvert.SerializeObject(academicDegrees);

            return new JsonResult(academicDegreesJson);
        }
        #endregion

        #region Add
        [HttpGet]
        public IActionResult Add()
        {
            return PartialView(MagicStrings.ViewNames._Add, new AcademicDegreeViewModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Add(AcademicDegreeViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView(MagicStrings.ViewNames._Add, model);

            if (_dataUnitOfWork.BaseUow.AcademicDegreesRepository.GetExists(model.Name))
                ModelState.AddModelError(nameof(Localizer.RecordAlreadyExists), Localizer.RecordAlreadyExists);

            if (ModelState.IsValid)
            {
                try
                {
                    _dataUnitOfWork.BaseUow.AcademicDegreesRepository.Add(model);
                    _dataUnitOfWork.BaseUow.AcademicDegreesRepository.SaveChanges();

                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.AcademicDegrees, model.Id, GetControllerName(), GetActionName(), null);

                    return Json(new { success = true, notification = new Notification(NotificationTypes.Success, Localizer.Saved, string.Format(Localizer.SuccessfullySavedAndRecordName, model.Name)).ConvertToJson() });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(Localizer.Error), Localizer.Error);

                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Add, Tables.Base.AcademicDegrees, model.Id, GetControllerName(), GetActionName(), ex);
                }
            }

            return PartialView(MagicStrings.ViewNames._Add, model);
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int AcademicdegreeId)
        {
            AcademicDegreeViewModel academicDegreeViewModel = _dataUnitOfWork.BaseUow.AcademicDegreesRepository.GetById(AcademicdegreeId);
            return PartialView(MagicStrings.ViewNames._Edit, academicDegreeViewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(AcademicDegreeViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView(MagicStrings.ViewNames._Edit, model);

            try
            {
                _dataUnitOfWork.BaseUow.AcademicDegreesRepository.Update(model);
                _dataUnitOfWork.BaseUow.AcademicDegreesRepository.SaveChanges();

                _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.AcademicDegrees, model.Id, GetControllerName(), GetActionName(), null);

                return Json(new { success = true, notification = new Notification(NotificationTypes.Success, Localizer.Saved, string.Format(Localizer.SuccessfullySavedAndRecordName, model.Name)).ConvertToJson() });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(Localizer.Error), Localizer.Error);

                _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Edit, Tables.Base.AcademicDegrees, model.Id, GetControllerName(), GetActionName(), ex);
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
                AcademicDegree academicDegree = _dataUnitOfWork.BaseUow.AcademicDegreesRepository.GetById(Id);
                if (academicDegree != null)
                {
                    _dataUnitOfWork.BaseUow.AcademicDegreesRepository.Remove(academicDegree);
                    _dataUnitOfWork.BaseUow.AcademicDegreesRepository.SaveChanges();

                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.AcademicDegrees, academicDegree.Id, GetControllerName(), GetActionName(), null);

                    notification = new Notification(NotificationTypes.Success, Localizer.Saved, string.Format(Localizer.SuccessfullySavedAndRecordName, academicDegree.Name));
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
            List<AcademicDegree> modelList = new List<AcademicDegree>();
            Notification notification = null;
            try
            {
                foreach (var item in list)
                {
                    modelList.Add(_dataUnitOfWork.BaseUow.AcademicDegreesRepository.GetById(item));
                }
                foreach (var item in modelList)
                {
                    _dataUnitOfWork.BaseUow.AcademicDegreesRepository.Remove(item);
                }
                _dataUnitOfWork.BaseUow.AcademicDegreesRepository.SaveChanges();

                foreach (var item in modelList)
                {
                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.AcademicDegrees, item.Id, GetControllerName(), GetActionName(), null);
                }
                notification = new Notification(NotificationTypes.Success, Localizer.Removed, string.Format(Localizer.SuccessfullyRemovedName, Localizer.AcademicDegrees));
            }
            catch (Exception ex)
            {
                notification = new Notification(NotificationTypes.Error, Localizer.ErrorFriendly, Localizer.AnErrorOccurredFriendly);
                foreach (var item in list)
                {
                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.AcademicDegrees, item, GetControllerName(), GetActionName(), ex);
                }
            }
            return Json(notification.ConvertToJson());

        }
        #endregion
    }
}