using Core.Constants;
using Core.Entities.Base;
using Core.Entities.Base.DTO;
using DAL;
using DAL.Repositories.Base.IRepository;
using DAL.Repositories.Base.IRepository.DTO;
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
    public class CitiesController : BaseController
    {
        #region Properties

        private readonly IDataUnitOfWork _dataUnitOfWork;

        private Localizer _localizer;
        private ISelectListHelper _selectList;
        private ILogger _logger;
        #endregion

        public CitiesController(IDataUnitOfWork dataUnitOfWork, Localizer localizer, ISelectListHelper selectList, ILogger logger, IBreadcrumb breadcrumb, IStringLocalizerFactory stringLocalizerFactory) : base(breadcrumb, stringLocalizerFactory)
        {
            _dataUnitOfWork = dataUnitOfWork;

            _localizer = localizer;
            _selectList = selectList;
            _logger = logger;
        }

        #region Index
        [HttpGet]
        public JsonResult JsonIndex()
        {
            IEnumerable<CityDTO> cities = _dataUnitOfWork.BaseUow.CitiesDTORepository.GetAll();

            var citiesJson = JsonConvert.SerializeObject(cities);

            return new JsonResult(citiesJson);
        }
        #endregion

        #region Add

        [HttpGet]
        public IActionResult Add()
        {
            return PartialView(MagicStrings.ViewNames._Add, new CityViewModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Add(CityViewModel model)
        {
            if(!ModelState.IsValid)
                return PartialView(MagicStrings.ViewNames._Add, model);

            if (_dataUnitOfWork.BaseUow.CitiesRepository.GetExists(model.Name, model.CountryId))
                ModelState.AddModelError(nameof(_localizer.RecordAlreadyExists), _localizer.RecordAlreadyExists);

            if (ModelState.IsValid)
            {
                try
                {
                    _dataUnitOfWork.BaseUow.CitiesRepository.Add(model);
                    _dataUnitOfWork.BaseUow.CitiesRepository.SaveChanges();

                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.Cities, model.Id, GetControllerName(), GetActionName(), null);

                    CityDTO readModel = _dataUnitOfWork.BaseUow.CitiesDTORepository.GetById(model.Id);

                    return Json(new { model = JsonConvert.SerializeObject(readModel), notification = new Notification(NotificationTypes.Success, _localizer.Saved, string.Format(_localizer.SuccessfullySavedAndRecordName, model.Name)).ConvertToJson() });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(_localizer.Error), _localizer.Error);

                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Add, Tables.Base.Cities, 0, GetControllerName(), GetActionName(), ex);
                }
            }

            return PartialView(MagicStrings.ViewNames._Add, model);
        }

        #endregion

        #region Edit
        public IActionResult Edit(int id)
        {
            CityViewModel model = _dataUnitOfWork.BaseUow.CitiesRepository.GetById(id);
            return PartialView(MagicStrings.ViewNames._Edit, model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(CityViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView(MagicStrings.ViewNames._Edit, model);

            if (_dataUnitOfWork.BaseUow.CitiesRepository.GetExists(model.Name,model.CountryId))
                ModelState.AddModelError(nameof(_localizer.RecordAlreadyExists), _localizer.RecordAlreadyExists);

            if (ModelState.IsValid)
            {
                try
                {
                    _dataUnitOfWork.BaseUow.CitiesRepository.Update(model);
                    _dataUnitOfWork.BaseUow.CitiesRepository.SaveChanges();

                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.Cities, model.Id, GetControllerName(), GetActionName(), null);

                    return Json(new { success = true, notification = new Notification(NotificationTypes.Success, _localizer.Saved, string.Format(_localizer.SuccessfullySavedAndRecordName, model.Name)).ConvertToJson() });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(_localizer.Error), _localizer.Error);

                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Edit, Tables.Base.Cities, model.Id, GetControllerName(), GetActionName(), ex);
                }
            }

            return PartialView(MagicStrings.ViewNames._Edit, model);
        }
        #endregion

        #region Delete 
        public JsonResult Delete(int Id)//5
        {
            Notification notification = null;

            try
            {
                City model = _dataUnitOfWork.BaseUow.CitiesRepository.GetById(Id);

                _dataUnitOfWork.BaseUow.CitiesRepository.Remove(model);
                _dataUnitOfWork.BaseUow.CitiesRepository.SaveChanges();

                _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.Cities, model.Id, GetControllerName(), GetActionName(), null);

                notification = new Notification(NotificationTypes.Success, _localizer.Delete, string.Format(_localizer.SuccessfullyRemovedName, model.Name));
            }
            catch (Exception ex)
            {
                notification = new Notification(NotificationTypes.Error, _localizer.ErrorFriendly, _localizer.AnErrorOccurredFriendly);
                _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.Cities, Id, GetControllerName(), GetActionName(), ex);
            }

            return Json(notification.ConvertToJson());
        }
        public JsonResult DeleteAll(List<int> list)
        {
            /* TO DO : Remove by csv values ! */
            List<City> modelList = new List<City>();
            Notification notification = null;
            try
            {
                for (int i = 0; i < list.Count; i++)
                {
                    modelList.Add(_dataUnitOfWork.BaseUow.CitiesRepository.GetById(list[i]));
                }
                for (int i = 0; i < modelList.Count; i++)
                {
                    _dataUnitOfWork.BaseUow.CitiesRepository.Remove(modelList[i]);
                }
                _dataUnitOfWork.BaseUow.CitiesRepository.SaveChanges();
                for (int i = 0; i < modelList.Count; i++)
                {
                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.Cities, modelList[i].Id, GetControllerName(), GetActionName(), null);
                }
                notification = new Notification(NotificationTypes.Success, Localizer.Removed, string.Format(Localizer.SuccessfullyRemovedName, Localizer.Cities));
            }
            catch (Exception ex)
            {
                notification = new Notification(NotificationTypes.Error, _localizer.ErrorFriendly, _localizer.AnErrorOccurredFriendly);
                for (int i = 0; i < modelList.Count; i++)
                {
                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.Cities,list[i], GetControllerName(), GetActionName(), ex);
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

        public JsonResult GetRegionsByCountryId(int countryId)
        {
            List<SelectListItem> regions = _selectList.RegionsByCountryID(countryId);

            return new JsonResult(regions);
        }
        public JsonResult GetCitiesByCountryId(int countryId)
        {
            List<SelectListItem> cities = _selectList.CitiesByCountryID(countryId);

            return new JsonResult(cities);
        }
        public JsonResult GetCitiesSelectListByRegionId(int regionId)
        {
            List<SelectListItem> cities = _selectList.CitiesByRegionID(regionId);

            return new JsonResult(cities);
        }
        public JsonResult GetCities(int countryId)
        {
            List<SelectListItem> cities = _selectList.Cities();

            return new JsonResult(cities);
        }

        #endregion
    }
}