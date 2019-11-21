using Core.Constants;
using Core.Entities.Base;
using Core.Entities.Base.DTO;
using DAL;
using DAL.Repositories.Base.IRepository;
using nevladinaOrg.Web.Areas.Administration.ViewModels;
using nevladinaOrg.Web.Constants;
using nevladinaOrg.Web.Controllers;
using nevladinaOrg.Web.Helpers;
using nevladinaOrg.Web.Helpers.Breadcrumb;
using nevladinaOrg.Web.Helpers.ImageHelper;
using nevladinaOrg.Web.Helpers.Logger;
using nevladinaOrg.Web.Helpers.SelectListHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace nevladinaOrg.Web.Areas.Administration.Controllers
{
    [Area(MagicStrings.AreaNames.Administration)]
    [Authorization]
    public class InstitutionsController : BaseController
    {
        #region Parameters
        private readonly IDataUnitOfWork _dataUnitOfWork;
        private Localizer _localizer;
        private ILogger _logger;
        private IImageHelper _imageHelper;
        private IHostingEnvironment _hostingEnvironment;
        #endregion
        public InstitutionsController(IDataUnitOfWork dataUnitOfWork,
                                    Localizer localizer,
                                    ILogger logger,
                                    IImageHelper imageHelper,
                                    IHostingEnvironment hostingEnvironment,
                                    IBreadcrumb breadcrumb, IStringLocalizerFactory stringLocalizerFactory)
                    : base(breadcrumb, stringLocalizerFactory)
        {
            _dataUnitOfWork = dataUnitOfWork;
            _localizer = localizer;
            _logger = logger;
            _imageHelper = imageHelper;
            _hostingEnvironment = hostingEnvironment;
        }
        #region Index
        [HttpGet]
        public JsonResult JsonIndex()
        {
            IEnumerable<InstitutionDTO> institutions = _dataUnitOfWork.BaseUow.InstitutionsDTORepository.GetAll();
            var institutionDTOJson = JsonConvert.SerializeObject(institutions);
            return new JsonResult(institutionDTOJson);
        }
        #endregion
        #region Add
        [HttpGet]
        public IActionResult Add()
        {
            var model = new InstitutionViewModel { Active = true };
            return PartialView(MagicStrings.ViewNames._Add, model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Add(InstitutionViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView(MagicStrings.ViewNames._Add, model);
            if (_dataUnitOfWork.BaseUow.InstitutionsRepository.GetExists(model.Name, model.CountryId, model.CityId, model.InstitutionTypeId))
                ModelState.AddModelError(nameof(_localizer.RecordAlreadyExists), _localizer.RecordAlreadyExists);
            if (ModelState.IsValid)
            {
                using (IDbContextTransaction contextTransaction = _dataUnitOfWork.BaseUow.InstitutionsRepository.BeginTransaction())
                {
                    try
                    {
                        if (model.ParentId == 0)
                            model.ParentId = null;
                        Institution saveModel = model;
                        _dataUnitOfWork.BaseUow.InstitutionsRepository.Add(saveModel);
                        _dataUnitOfWork.BaseUow.InstitutionsRepository.SaveChanges();
                        contextTransaction.Commit();
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.Institutions, model.Id, GetControllerName(), GetActionName(), null);
                        return Json(new { success = true, notification = new Notification(NotificationTypes.Success, _localizer.Saved, string.Format(_localizer.SuccessfullySavedAndRecordName, model.Name)).ConvertToJson() });
                    }
                    catch (Exception ex)
                    {
                        contextTransaction.Rollback();
                        ModelState.AddModelError(nameof(_localizer.Error), _localizer.Error);
                        _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Add, Tables.Base.Institutions, model.Id, GetControllerName(), GetActionName(), ex);
                    }
                }
            }
            return PartialView(MagicStrings.ViewNames._Add, model);
        }
        #endregion
        #region Edit
        public async Task<IActionResult> Edit(int id)
        {
            InstitutionViewModel model = _dataUnitOfWork.BaseUow.InstitutionsRepository.GetById(id);
            return PartialView(MagicStrings.ViewNames._Edit, model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(InstitutionViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView(MagicStrings.ViewNames._Edit, model);
            if (ModelState.IsValid)
            {
                using (IDbContextTransaction contextTransaction = _dataUnitOfWork.BaseUow.InstitutionsRepository.BeginTransaction())
                {
                    try
                    {
                        if (model.ParentId == 0)
                            model.ParentId = null;
                        _dataUnitOfWork.BaseUow.InstitutionsRepository.Update(model);
                        _dataUnitOfWork.BaseUow.InstitutionsRepository.SaveChanges();
                        Institution baseInstitution = new Institution();
                        baseInstitution = _dataUnitOfWork.BaseUow.InstitutionsRepository.GetById(model.Id);
                        baseInstitution.Active = model.Active;
                        _dataUnitOfWork.BaseUow.InstitutionsRepository.Update(baseInstitution);
                        _dataUnitOfWork.BaseUow.InstitutionsRepository.SaveChanges();
                        contextTransaction.Commit();
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.Institutions, model.Id, GetControllerName(), GetActionName(), null);
                        return Json(new { success = true, notification = new Notification(NotificationTypes.Success, _localizer.Saved, string.Format(_localizer.SuccessfullySavedAndRecordName, model.Name)).ConvertToJson() });
                    }
                    catch (Exception ex)
                    {
                        contextTransaction.Rollback();
                        ModelState.AddModelError(nameof(_localizer.Error), _localizer.Error);
                        _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Edit, Tables.Base.Institutions, model.Id, GetControllerName(), GetActionName(), ex);
                    }
                }
            }
            return PartialView(MagicStrings.ViewNames._Edit, model);
        }
        #endregion
        #region Delete 
        public JsonResult Delete(int Id)
        {
            Notification notification = null;
            using (IDbContextTransaction contextTransaction = _dataUnitOfWork.BaseUow.InstitutionsRepository.BeginTransaction())
            {
                try
                {
                    Institution model = _dataUnitOfWork.BaseUow.InstitutionsRepository.GetById(Id);
                    Institution baseModel = _dataUnitOfWork.BaseUow.InstitutionsRepository.GetById(Id);
                    _dataUnitOfWork.BaseUow.InstitutionsRepository.Remove(model);
                    _dataUnitOfWork.BaseUow.InstitutionsRepository.SaveChanges();
                    _dataUnitOfWork.BaseUow.InstitutionsRepository.Remove(baseModel);
                    _dataUnitOfWork.BaseUow.InstitutionsRepository.SaveChanges();
                    contextTransaction.Commit();
                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.Institutions, model.Id, GetControllerName(), GetActionName(), null);
                    notification = new Notification(NotificationTypes.Success, _localizer.Saved, string.Format(_localizer.SuccessfullyRemovedName, model.Name));
                }
                catch (Exception ex)
                {
                    contextTransaction.Rollback();
                    notification = new Notification(NotificationTypes.Error, Localizer.ErrorFriendly, Localizer.AnErrorOccurredFriendly);
                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.Institutions, Id, GetControllerName(), GetActionName(), ex);
                }
            }
            return Json(notification.ConvertToJson());
        }
        public JsonResult DeleteAll(List<int> list)
        {
            var modelList = new List<Institution>();
            Notification notification = null;
            using (IDbContextTransaction contextTransaction = _dataUnitOfWork.BaseUow.InstitutionsRepository.BeginTransaction())
            {
                try
                {
                    foreach (var item in list)
                    {
                        modelList.Add(_dataUnitOfWork.BaseUow.InstitutionsRepository.GetById(item));
                    }
                    foreach (var item in modelList)
                    {
                        _dataUnitOfWork.BaseUow.InstitutionsRepository.Remove(item);
                    }
                    foreach (var item in modelList)
                    {
                        _dataUnitOfWork.BaseUow.InstitutionsRepository.Remove(item);
                    }
                    _dataUnitOfWork.BaseUow.InstitutionsRepository.SaveChanges();
                    contextTransaction.Commit();
                    foreach (var item in modelList)
                    {
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.Institutions, item.Id, GetControllerName(), GetActionName(), null);
                    }
                    notification = new Notification(NotificationTypes.Success, Localizer.Removed, string.Format(Localizer.SuccessfullyRemovedName, Localizer.Institutions));
                }
                catch (Exception ex)
                {
                    contextTransaction.Rollback();
                    notification = new Notification(NotificationTypes.Error, Localizer.ErrorFriendly, Localizer.AnErrorOccurredFriendly);
                    foreach (var item in list)
                    {
                        _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.Institutions, item, GetControllerName(), GetActionName(), ex);
                    }
                }
            }
            return Json(notification.ConvertToJson());
        }
        #endregion
    }
}