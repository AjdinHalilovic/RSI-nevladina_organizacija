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
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
namespace nevladinaOrg.Web.Areas.Administration.Controllers
{
    [Area(MagicStrings.AreaNames.Administration)]
    [Authorization]
    public class RolesController : BaseController
    {
        #region Properties
        private IDataUnitOfWork _dataUnitOfWork;
        private Localizer _localizer;
        private ILogger _logger;
        #endregion
        public RolesController(IDataUnitOfWork dataUnitOfWork,
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
            IEnumerable<Role> roles = _dataUnitOfWork.BaseUow.RolesRepository.GetAll();
            var rolesJson = JsonConvert.SerializeObject(roles);
            return new JsonResult(rolesJson);
        }
        #endregion
        #region Add
        [Authorization(Enumerations.Functionalities.Add)]
        public IActionResult Add(Enumerations.SaveAndOptions option)
        {
            return PartialView(MagicStrings.ViewNames._Add, new RoleFunctionalityViewModel { SaveAndOptions = option });
        }
        [HttpPost, ValidateAntiForgeryToken, Authorization(Enumerations.Functionalities.Add)]
        public IActionResult Add(RoleFunctionalityViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView(MagicStrings.ViewNames._Add, model);
            if (_dataUnitOfWork.BaseUow.RolesRepository.GetExists(model.Role.Name))
                ModelState.AddModelError(nameof(_localizer.RecordAlreadyExists), _localizer.RecordAlreadyExists);
            if (ModelState.IsValid)
            {
                using (IDbContextTransaction contextTransaction = _dataUnitOfWork.BaseUow.RolesRepository.BeginTransaction())
                {
                    try
                    {
                        Role role = new Role();
                        role = model.Role;
                        _dataUnitOfWork.BaseUow.RolesRepository.Add(role);
                        _dataUnitOfWork.BaseUow.RolesRepository.SaveChanges();
                        string[] string_Funct_Ids = model.FunctionalityIds.Split(",");
                        List<int> Funct_Ids = new List<int>();
                        foreach (var item in string_Funct_Ids)
                        {
                            Funct_Ids.Add(int.Parse(item));
                        }
                        foreach (var item in Funct_Ids)
                        {
                            RoleFunctionality obj = new RoleFunctionality
                            {
                                RoleId = role.Id,
                                FunctionalityId = item,
                                AssignmentDate = DateTime.Now
                            };
                            _dataUnitOfWork.BaseUow.RolesFunctionalitiesRepository.Add(obj);
                        }
                        _dataUnitOfWork.BaseUow.RolesFunctionalitiesRepository.SaveChanges();
                        contextTransaction.Commit();
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.Roles, model.Role.Id, GetControllerName(), GetActionName(), null);
                        foreach (var item in Funct_Ids)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.RoleFunctionalities, item, GetControllerName(), GetActionName(), null);
                        }
                        return Json(new { success = true, notification = new Notification(NotificationTypes.Success, _localizer.Saved, string.Format(_localizer.SuccessfullySavedAndRecordName, model.Role.Name)).ConvertToJson() });
                    }
                    catch (Exception ex)
                    {
                        contextTransaction.Rollback();
                        ModelState.AddModelError(nameof(_localizer.Error), _localizer.Error);
                        _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Add, Tables.Base.Roles, model.Role.Id, GetControllerName(), GetActionName(), ex);
                    }
                }
            }
            return PartialView(MagicStrings.ViewNames._Add, model);
        }
        #endregion
        #region Edit
        [Authorization(Enumerations.Functionalities.Edit)]
        public IActionResult Edit(int Id)
        {
            RoleViewModel roleModel = _dataUnitOfWork.BaseUow.RolesRepository.GetById(Id);
            RoleFunctionalityViewModel model = new RoleFunctionalityViewModel
            {
                Role = roleModel
            };
            IEnumerable<int> functionalityIds = _dataUnitOfWork.BaseUow.RolesFunctionalitiesRepository.FunctionalityIdsByRoleId(Id);
            string stringFunctionalities = string.Empty;
            foreach (var item in functionalityIds)
            {
                stringFunctionalities += item + ",";
            }
            model.FunctionalityIds = stringFunctionalities;
            return PartialView(MagicStrings.ViewNames._Edit, model);
        }
        [HttpPost, ValidateAntiForgeryToken, Authorization(Enumerations.Functionalities.Edit)]
        public IActionResult Edit(RoleFunctionalityViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView(MagicStrings.ViewNames._Edit, model);
            if (ModelState.IsValid)
            {
                using (IDbContextTransaction contextTransaction = _dataUnitOfWork.BaseUow.RolesRepository.BeginTransaction())
                {
                    try
                    {
                        Role role = new Role();
                        role = model.Role;
                        _dataUnitOfWork.BaseUow.RolesRepository.Update(role);
                        _dataUnitOfWork.BaseUow.RolesRepository.SaveChanges();
                        string[] string_Funct_Ids = model.FunctionalityIds.Split(",");
                        List<int> Funct_Ids = new List<int>();
                        foreach (var item in string_Funct_Ids)
                        {
                            Funct_Ids.Add(int.Parse(item));
                        }
                        IEnumerable<int> oldRoleFunctionalities = _dataUnitOfWork.BaseUow.RolesFunctionalitiesRepository.FunctionalityIdsByRoleId(role.Id);
                        bool exist;
                        foreach (var item in Funct_Ids)
                        {
                            exist = false;
                            foreach (var item1 in oldRoleFunctionalities)
                            {
                                if (item == item1)
                                {
                                    exist = true;
                                }
                            }
                            if (!exist)
                            {
                                RoleFunctionality obj = new RoleFunctionality
                                {
                                    RoleId = model.Role.Id,
                                    FunctionalityId = item,
                                    AssignmentDate = DateTime.Now
                                };
                                _dataUnitOfWork.BaseUow.RolesFunctionalitiesRepository.Add(obj);
                            }
                        }
                        foreach (var item in oldRoleFunctionalities)
                        {
                            exist = false;
                            foreach (var item1 in Funct_Ids)
                            {
                                if (item == item1)
                                {
                                    exist = true;
                                }
                            }
                            if (!exist)
                            {
                                RoleFunctionality obj = _dataUnitOfWork.BaseUow.RolesFunctionalitiesRepository.GetByFunctionalityId(item, role.Id);
                                obj.ModifiedDate = DateTime.Now;
                                _dataUnitOfWork.BaseUow.RolesFunctionalitiesRepository.Remove(obj);
                            }
                        }
                        _dataUnitOfWork.BaseUow.RolesFunctionalitiesRepository.SaveChanges();
                        contextTransaction.Commit();
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.Roles, model.Role.Id, GetControllerName(), GetActionName(), null);
                        return Json(new { success = true, notification = new Notification(NotificationTypes.Success, _localizer.Saved, string.Format(_localizer.SuccessfullySavedAndRecordName, model.Role.Name)).ConvertToJson() });
                    }
                    catch (Exception ex)
                    {
                        contextTransaction.Rollback();
                        ModelState.AddModelError(nameof(_localizer.Error), _localizer.Error);
                        _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Edit, Tables.Base.Roles, model.Role.Id, GetControllerName(), GetActionName(), ex);
                    }
                }
            }
            return PartialView(MagicStrings.ViewNames._Edit, model);
        }
        #endregion
        #region Delete 
        [Authorization(Enumerations.Functionalities.Delete)]
        public JsonResult Delete(int Id)
        {
            Notification notification = null;
            using (IDbContextTransaction contextTransaction = _dataUnitOfWork.BaseUow.RolesRepository.BeginTransaction())
            {
                try
                {
                    IEnumerable<RoleFunctionality> roleFunctionalities = _dataUnitOfWork.BaseUow.RolesFunctionalitiesRepository.GetFunctionalitiesByRoleId(Id);
                    foreach (var item in roleFunctionalities)
                    {
                        _dataUnitOfWork.BaseUow.RolesFunctionalitiesRepository.Remove(item);
                    }
                    Role model = _dataUnitOfWork.BaseUow.RolesRepository.GetById(Id);
                    _dataUnitOfWork.BaseUow.RolesRepository.Remove(model);
                    _dataUnitOfWork.BaseUow.RolesFunctionalitiesRepository.SaveChanges();
                    _dataUnitOfWork.BaseUow.RolesRepository.SaveChanges();
                    contextTransaction.Commit();
                    foreach (var item in roleFunctionalities)
                    {
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.RoleFunctionalities, item.Id, GetControllerName(), GetActionName(), null);
                    }
                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.Roles, model.Id, GetControllerName(), GetActionName(), null);
                    notification = new Notification(NotificationTypes.Success, _localizer.Removed, string.Format(_localizer.SuccessfullyRemovedName, model.Name));
                }
                catch (Exception ex)
                {
                    contextTransaction.Rollback();
                    notification = new Notification(NotificationTypes.Error, Localizer.ErrorFriendly, Localizer.AnErrorOccurredFriendly);
                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.Roles, Id, GetControllerName(), GetActionName(), ex);
                }
            }
            return Json(notification.ConvertToJson());
        }
        public JsonResult DeleteAll(List<int> list)
        {
            List<Role> modelListRoles = new List<Role>();
            List<IEnumerable<RoleFunctionality>> modelListRoleFunctionalities = new List<IEnumerable<RoleFunctionality>>();
            Notification notification = null;
            using (IDbContextTransaction contextTransaction = _dataUnitOfWork.BaseUow.RolesRepository.BeginTransaction())
            {
                try
                {
                    foreach (var item in list)
                    {
                        modelListRoles.Add(_dataUnitOfWork.BaseUow.RolesRepository.GetById(item));
                        IEnumerable<RoleFunctionality> roleFunctionalities = _dataUnitOfWork.BaseUow.RolesFunctionalitiesRepository.GetFunctionalitiesByRoleId(item);
                        modelListRoleFunctionalities.Add(roleFunctionalities);
                    }
                    foreach (var item in modelListRoleFunctionalities)
                    {
                        foreach (var item1 in item)
                        {
                            _dataUnitOfWork.BaseUow.RolesFunctionalitiesRepository.Remove(item1);
                        }
                    }
                    foreach (var item in modelListRoles)
                    {
                        _dataUnitOfWork.BaseUow.RolesRepository.Remove(item);
                    }
                    _dataUnitOfWork.BaseUow.RolesFunctionalitiesRepository.SaveChanges();
                    _dataUnitOfWork.BaseUow.RolesRepository.SaveChanges();
                    contextTransaction.Commit();
                    foreach (var item in modelListRoleFunctionalities)
                    {
                        foreach (var item1 in item)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.RoleFunctionalities, item1.Id, GetControllerName(), GetActionName(), null);
                        }
                    }
                    foreach (var item in modelListRoles)
                    {
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.Roles, item.Id, GetControllerName(), GetActionName(), null);
                    }
                    notification = new Notification(NotificationTypes.Success, Localizer.Removed, string.Format(Localizer.SuccessfullyRemovedName, Localizer.Roles));
                }
                catch (Exception ex)
                {
                    contextTransaction.Rollback();
                    notification = new Notification(NotificationTypes.Error, Localizer.ErrorFriendly, Localizer.AnErrorOccurredFriendly);
                    foreach (var item in list)
                    {
                        _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.Roles, item, GetControllerName(), GetActionName(), ex);
                    }
                }
            }
            return Json(notification.ConvertToJson());
        }
        #endregion
        #region Helpers
        public JsonResult GetFunctionalities()
        {
            IEnumerable<Functionality> functionalities = _dataUnitOfWork.BaseUow.FunctionalitiesRepository.GetAll();
            var functionalitiesJson = JsonConvert.SerializeObject(functionalities);
            return new JsonResult(functionalitiesJson);
        }
        #endregion
    }
}