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
    public class ContactTypesController : BaseController
    {
        #region Properties
        private readonly IDataUnitOfWork _datUnitOfWork;

        private readonly ILogger _logger;
        #endregion

        public ContactTypesController(
            IDataUnitOfWork datUnitOfWork,
            IBreadcrumb breadcrumb,
            IStringLocalizerFactory stringLocalizerFactory,
            ILogger logger)
            : base(breadcrumb, stringLocalizerFactory)
        {
            _datUnitOfWork = datUnitOfWork;

            _logger = logger;
        }

        #region Index
        public JsonResult JsonIndex()
        {
            IEnumerable<ContactType> contactTypes = _datUnitOfWork.BaseUow.ContactTypesRepository.GetAll();

            var contactTypesJson = JsonConvert.SerializeObject(contactTypes);

            return new JsonResult(contactTypesJson);
        }
        #endregion

        #region Add
        [HttpGet]
        public IActionResult Add()
        {
            return PartialView(MagicStrings.ViewNames._Add, new ContactTypeViewModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Add(ContactTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView(MagicStrings.ViewNames._Add, model);

            if (_datUnitOfWork.BaseUow.ContactTypesRepository.GetExists(model.Name))
                ModelState.AddModelError(nameof(Localizer.RecordAlreadyExists), Localizer.RecordAlreadyExists);

            if (ModelState.IsValid)
            {
                try
                {
                    _datUnitOfWork.BaseUow.ContactTypesRepository.Add(model);
                    _datUnitOfWork.BaseUow.ContactTypesRepository.SaveChanges();

                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.ContactTypes, model.Id, GetControllerName(), GetActionName(), null);

                    return Json(new { success = true, notification = new Notification(NotificationTypes.Success, Localizer.Saved, string.Format(Localizer.SuccessfullySavedAndRecordName, model.Name)).ConvertToJson() });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(Localizer.Error), Localizer.Error);

                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Add, Tables.Base.ContactTypes, model.Id, GetControllerName(), GetActionName(), ex);
                }
            }

            return PartialView(MagicStrings.ViewNames._Add, model);
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int ContactTypeId)
        {
            ContactTypeViewModel contactTypeViewModel = _datUnitOfWork.BaseUow.ContactTypesRepository.GetById(ContactTypeId);
            return PartialView(MagicStrings.ViewNames._Edit, contactTypeViewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(ContactTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView(MagicStrings.ViewNames._Edit, model);

            try
            {
                _datUnitOfWork.BaseUow.ContactTypesRepository.Update(model);
                _datUnitOfWork.BaseUow.ContactTypesRepository.SaveChanges();

                _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.ContactTypes, model.Id, GetControllerName(), GetActionName(), null);

                return Json(new { success = true, notification = new Notification(NotificationTypes.Success, Localizer.Saved, string.Format(Localizer.SuccessfullySavedAndRecordName, model.Name)).ConvertToJson() });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(Localizer.Error), Localizer.Error);

                _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Edit, Tables.Base.ContactTypes, model.Id, GetControllerName(), GetActionName(), ex);
            }

            return PartialView(MagicStrings.ViewNames._Edit, model);
        }
        #endregion

        #region Delete
        public JsonResult Delete(int Id)
        {
            ContactType contactType = _datUnitOfWork.BaseUow.ContactTypesRepository.GetById(Id);
            Notification notification = null;
            try
            {
                if (contactType != null)
                {
                    _datUnitOfWork.BaseUow.ContactTypesRepository.Remove(contactType);
                    _datUnitOfWork.BaseUow.ContactTypesRepository.SaveChanges();

                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.ContactTypes, Id, GetControllerName(), GetActionName(), null);
                    notification = new Notification(NotificationTypes.Success, Localizer.Removed, string.Format(Localizer.SuccessfullyRemovedName, contactType.Name));
                }
            }
            catch (Exception ex)
            {
                notification = new Notification(NotificationTypes.Error, Localizer.Error, Localizer.AnErrorOccurredFriendly);
                _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.ContactTypes, Id, GetControllerName(), GetActionName(), ex);
            }

            return Json(notification.ConvertToJson());
        }

        public JsonResult DeleteAll(List<int> list)
        {
            List<ContactType> modelList = new List<ContactType>();
            Notification notification = null;
            try
            {
                foreach (var item in list)
                {
                    modelList.Add(_datUnitOfWork.BaseUow.ContactTypesRepository.GetById(item));
                }
                _datUnitOfWork.BaseUow.ContactTypesRepository.RemoveRange(modelList);
                _datUnitOfWork.BaseUow.ContactTypesRepository.SaveChanges();
                foreach (var item in modelList)
                {
                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.ContactTypes, item.Id, GetControllerName(), GetActionName(), null);
                }
                notification = new Notification(NotificationTypes.Success, Localizer.Removed, string.Format(Localizer.SuccessfullyRemovedName, Localizer.ContactTypes));
            }
            catch (Exception ex)
            {
                notification = new Notification(NotificationTypes.Error, Localizer.ErrorFriendly, Localizer.AnErrorOccurredFriendly);
                foreach (var item in list)
                {
                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.ContactTypes, item, GetControllerName(), GetActionName(), ex);
                }
            }
            return Json(notification.ConvertToJson());
        }
        #endregion
    }
}