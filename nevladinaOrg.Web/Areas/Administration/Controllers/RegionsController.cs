using Core.Constants;
using Core.Entities.Base;
using Core.Entities.Base.DTO;
using DAL;
using nevladinaOrg.Web.Areas.Administration.ViewModels;
using nevladinaOrg.Web.Constants;
using nevladinaOrg.Web.Controllers;
using nevladinaOrg.Web.Helpers;
using nevladinaOrg.Web.Helpers.Breadcrumb;
using nevladinaOrg.Web.Helpers.Logger;
using nevladinaOrg.Web.Helpers.SelectListHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace nevladinaOrg.Web.Areas.Administration.Controllers
{
    [Area(MagicStrings.AreaNames.Administration)]
    [Authorization]
    public class RegionsController : BaseController
    {
        #region Properties
        private readonly IDataUnitOfWork _dataUnitOfWork;

        private readonly ISelectListHelper _selectList;
        private readonly ILogger _logger;
        #endregion

        public RegionsController(
            IDataUnitOfWork dataUnitOfWork,
            IBreadcrumb breadcrumb,
            IStringLocalizerFactory stringLocalizerFactory,
            ILogger logger,
            ISelectListHelper selectList
            )
            : base(breadcrumb, stringLocalizerFactory)
        {
            _dataUnitOfWork = dataUnitOfWork;

            _logger = logger;
            _selectList = selectList;
        }

        #region Index

        [HttpGet]
        public JsonResult JsonIndex()
        {
            IEnumerable<RegionDTO> regions = _dataUnitOfWork.BaseUow.RegionsDTORepository.GetAll();

            var regionsJson = JsonConvert.SerializeObject(regions);

            return new JsonResult(regionsJson);
        }

        #endregion


        #region Add
        [HttpGet]
        public IActionResult Add(Enumerations.SaveAndOptions option)
        {
            return PartialView(MagicStrings.ViewNames._Add, new RegionViewModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Add(RegionViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView(MagicStrings.ViewNames._Add, model);

            if (_dataUnitOfWork.BaseUow.RegionsRepository.GetExists(model.Name, model.CountryId))
                ModelState.AddModelError(nameof(Localizer.RecordAlreadyExists), Localizer.RecordAlreadyExists);

            if (ModelState.IsValid)
            {
                try
                {
                    _dataUnitOfWork.BaseUow.RegionsRepository.Add(model);
                    _dataUnitOfWork.BaseUow.RegionsRepository.SaveChanges();

                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.Regions, model.Id, GetControllerName(), GetActionName(), null);

                    return Json(new { success = true, notification = new Notification(NotificationTypes.Success, Localizer.Saved, string.Format(Localizer.SuccessfullySavedAndRecordName, model.Name)).ConvertToJson() });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(Localizer.Error), Localizer.Error);

                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Add, Tables.Base.Regions, model.Id, GetControllerName(), GetActionName(), ex);
                }
            }

            return PartialView(MagicStrings.ViewNames._Add, model);
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            RegionViewModel regionVM = _dataUnitOfWork.BaseUow.RegionsRepository.GetById(Id);
            return PartialView(MagicStrings.ViewNames._Edit, regionVM);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(RegionViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView(MagicStrings.ViewNames._Edit, model);

            if (ModelState.IsValid)
            {
                try
                {
                    _dataUnitOfWork.BaseUow.RegionsRepository.Update(model);
                    _dataUnitOfWork.BaseUow.RegionsRepository.SaveChanges();

                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.Regions, model.Id, GetControllerName(), GetActionName(), null);

                    return Json(new { success = true, notification = new Notification(NotificationTypes.Success, Localizer.Saved, string.Format(Localizer.SuccessfullySavedAndRecordName, model.Name)).ConvertToJson() });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(Localizer.Error), Localizer.Error);

                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Edit, Tables.Base.Regions, model.Id, GetControllerName(), GetActionName(), ex);
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
                var region = _dataUnitOfWork.BaseUow.RegionsRepository.GetById(Id);

                _dataUnitOfWork.BaseUow.RegionsRepository.Remove(region);
                _dataUnitOfWork.BaseUow.RegionsRepository.SaveChanges();

                _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.Regions, region.Id, GetControllerName(), GetActionName(), null);
                notification = new Notification(NotificationTypes.Success, Localizer.Removed, string.Format(Localizer.SuccessfullyRemovedName, region.Name));
            }
            catch (Exception ex)
            {
                notification = new Notification(NotificationTypes.Error, Localizer.ErrorFriendly, Localizer.AnErrorOccurredFriendly);
                _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.Regions, Id, GetControllerName(), GetActionName(), ex);
            }

            return Json(notification.ConvertToJson());
        }
        public JsonResult DeleteAll(List<int> list)
        {
            /* TO DO : Remove by csv values ! */
            List<Region> modelList = new List<Region>();
            Notification notification = null;
            try
            {
                foreach (var item in list)
                {
                    modelList.Add(_dataUnitOfWork.BaseUow.RegionsRepository.GetById(item));
                }
                foreach (var item in modelList)
                {
                    _dataUnitOfWork.BaseUow.RegionsRepository.Remove(item);
                }
                _dataUnitOfWork.BaseUow.RegionsRepository.SaveChanges();
                foreach (var item in modelList)
                {
                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.Regions, item.Id, GetControllerName(), GetActionName(), null);
                }
                notification = new Notification(NotificationTypes.Success, Localizer.Removed, string.Format(Localizer.SuccessfullyRemovedName, Localizer.Regions));
            }
            catch (Exception ex)
            {
                notification = new Notification(NotificationTypes.Error, Localizer.ErrorFriendly, Localizer.AnErrorOccurredFriendly);

                foreach (var item in list)
                {
                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.Regions, item, GetControllerName(), GetActionName(), ex);
                }
            }
            return Json(notification.ConvertToJson());

        }
        #endregion


        #region Helpers - JSON
        public JsonResult GetRegions()
        {
            List<SelectListItem> regions = _selectList.Regions();

            return new JsonResult(regions);
        }

        public JsonResult GetRegionsSelectListByCountryId(int countryId)
        {
            List<SelectListItem> regions = _selectList.RegionsByCountryID(countryId);

            return new JsonResult(regions);
        }
        #endregion
    }
}