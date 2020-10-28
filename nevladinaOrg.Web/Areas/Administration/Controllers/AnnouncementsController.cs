using Core.Constants;
using Core.Entities.Base;
using DAL;
using DAL.Repositories.Base.IRepository;
using nevladinaOrg.Web.Areas.Administration.ViewModels;
using nevladinaOrg.Web.Constants;
using nevladinaOrg.Web.Controllers;
using nevladinaOrg.Web.Helpers;
using nevladinaOrg.Web.Helpers.Breadcrumb;
using nevladinaOrg.Web.Helpers.ImageHelper;
using nevladinaOrg.Web.Helpers.Logger;
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
    public class AnnouncementsController : BaseController
    {
        #region Parameters
        private IDataUnitOfWork _dataUnitOfWork;

        private Localizer _localizer;
        private ILogger _logger;
        private IImageHelper _imager;
        IHostingEnvironment _environment;
        #endregion

        public AnnouncementsController(
            IDataUnitOfWork dataUnitOfWork,
            IHostingEnvironment environment,
            Localizer localizer, ILogger logger, IImageHelper imager,
            IBreadcrumb breadcrumb,
            IStringLocalizerFactory stringLocalizerFactory)
            : base(breadcrumb, stringLocalizerFactory)
        {
            _dataUnitOfWork = dataUnitOfWork;

            _environment = environment;
            _localizer = localizer;
            _logger = logger;
            _imager = imager;
        }

        #region Index
        [HttpGet]
        public JsonResult JsonIndex()
        {
            IEnumerable<Announcement> announcements = _dataUnitOfWork.BaseUow.AnnouncementsRepository.GetAll();
            var announcementVM = new List<AnnouncementViewModel>();
            foreach (var item in announcements)
            {
                announcementVM.Add(item);
            }
            foreach (var item in announcementVM)
            {
                item.ModifiedDateString = item.ModifiedDate.ToShortDateString() + " " + item.ModifiedDate.ToShortTimeString();
            }
            var announcementsJson = JsonConvert.SerializeObject(announcementVM);

            return new JsonResult(announcementsJson);
        }
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region Add
        [HttpGet]
        public IActionResult Add(Enumerations.SaveAndOptions option)
        {
            #region DropzoneSession

            var sessionDocuments = HttpContext.Session.GetObject<Dictionary<string, string>>(SessionKeys.Documents);
            var sessionImages = HttpContext.Session.GetObject<Dictionary<string, string>>(SessionKeys.Images);

            var files = new List<string>();
            if (sessionImages != null)
            {
                files = sessionImages.Select(x => x.Key).ToList();
            }
            if (sessionDocuments != null)
            {
                files.AddRange(sessionDocuments.Select(x => x.Key).ToList());
            }
            HttpContext.Session.SetObject<Dictionary<string, string>>(SessionKeys.Images, new Dictionary<string, string>());
            HttpContext.Session.SetObject<Dictionary<string, string>>(SessionKeys.Documents, new Dictionary<string, string>());
            HttpContext.Session.SetObject<Dictionary<string, string>>(SessionKeys.DropzoneTempThumbnails, new Dictionary<string, string>());
            _imager.DropzoneCleaner(files);

            #endregion
            var model = new AnnouncementOrganizationsViewModel() { SaveAndOptions = option, SendToOrganizations = null, SendToUsers = null, SendArea = null };
            model.Announcement = new AnnouncementViewModel() { Active = true };
            model.AnnouncementDocuments = new List<AnnouncementDocument>();
            model.AnnouncementPhotos = new List<AnnouncementPhoto>();
            return PartialView(MagicStrings.ViewNames._Add, model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AnnouncementOrganizationsViewModel model)
        {
            #region Dropzone    
            var sessionDocuments = HttpContext.Session.GetObject<Dictionary<string, string>>(SessionKeys.Documents);
            var sessionImages = HttpContext.Session.GetObject<Dictionary<string, string>>(SessionKeys.Images);

            var files = new List<string>();
            if (sessionImages != null)
            {
                files = sessionImages.Select(x => x.Key).ToList();
            }
            if (sessionDocuments != null)
            {
                files.AddRange(sessionDocuments.Select(x => x.Key).ToList());
            }


            var di = new DirectoryInfo(_environment.WebRootPath + Paths.DropzoneTemp);
            var diThumb = new DirectoryInfo(_environment.WebRootPath + Paths.DropzoneTempThumbnails);
            foreach (var formFile in di.GetFiles())
            {
                if (files.Contains(formFile.Name))
                {
                    if (!Data.DocumentFormats.Contains(System.IO.Path.GetExtension(formFile.Extension.ToLower())) && !Data.PhotoFormats.Contains(System.IO.Path.GetExtension(formFile.Extension.ToLower())))
                    {
                        ModelState.AddModelError(nameof(Localizer.UploadTypeFileNotSupported), string.Format(Localizer.UploadTypeFileNotSupported, System.IO.Path.GetExtension(formFile.Extension)));
                        var fileThumb = diThumb.GetFiles().Where(x => x.Name == formFile.Name).FirstOrDefault();
                        if (fileThumb != null)
                            fileThumb.Delete();
                        sessionImages.Remove(sessionImages.Where(x => x.Key == formFile.Name).Select(x => x.Key).FirstOrDefault());
                        HttpContext.Session.SetObject<Dictionary<string, string>>(SessionKeys.Images, sessionImages);
                        formFile.Delete();
                    }
                    else if (Data.PhotoFormats.Contains(System.IO.Path.GetExtension(formFile.Extension)) && !model.AnnouncementPhotos.Select(x => x.Name).Contains(formFile.Name))
                        model.AnnouncementPhotos.Add(new AnnouncementPhoto { AnnouncementId = model.Announcement.Id, Photo = System.IO.File.ReadAllBytes(formFile.FullName), Name = formFile.Name, Size = formFile.Length, Type = formFile.Extension, Thumbnail = _imager.GetThumbnail(System.IO.File.ReadAllBytes(formFile.FullName), 120, 120) });
                    else
                    {
                        if (!model.AnnouncementDocuments.Select(x => x.Name).Contains(formFile.Name))
                            model.AnnouncementDocuments.Add(new AnnouncementDocument { AnnouncementId = model.Announcement.Id, Document = System.IO.File.ReadAllBytes(formFile.FullName), Name = formFile.Name, Size = formFile.Length, Type = formFile.Extension });
                    }
                }
            }
            #endregion
            if (model.SendArea == "Organizations")
            {
                if (model.SendToOrganizations == "forAllOrganization")
                {
                    /* TO-DO : All organizations of currently logged in institutionId */
                    model.OrganizationIds = _dataUnitOfWork.BaseUow.AnnouncementOrganizationsRepository.GetAll().Select(i => i.Id.ToString()).ToArray();
                    if (model.OrganizationIds == null)
                    {
                        ModelState.AddModelError(nameof(Localizer.NotFoundAnyOrganizationAsRecipient), Localizer.NotFoundAnyOrganizationAsRecipient);
                    }
                }
                else if (model.OrganizationIds == null)
                {
                    ModelState.AddModelError(nameof(Localizer.ErrorMessageSelectOrganizationAsRecipientMin1), Localizer.ErrorMessageSelectOrganizationAsRecipientMin1);
                }
            }
            else if (model.SendArea == "Users")
            {
                if (model.SendToUsers == "forAllUsers")
                {
                    /* TO-DO : All users of currently logged in organizationId or institutionId */
                    model.UserIds = _dataUnitOfWork.BaseUow.UsersRepository.GetAll().Where(x => x.Id != LoggedUserIds.UserId).Select(i => i.Id.ToString()).ToArray();
                    if (model.UserIds == null)
                    {
                        ModelState.AddModelError(nameof(Localizer.NotFoundAnyUsersAsRecipient), Localizer.NotFoundAnyUsersAsRecipient);
                    }
                }
                else if (model.UserIds == null)
                {
                    ModelState.AddModelError(nameof(Localizer.ErrorMessageSelectUserAsRecipientMin1), Localizer.ErrorMessageSelectUserAsRecipientMin1);
                }
            }

            if (_dataUnitOfWork.BaseUow.AnnouncementsRepository.GetExists(model.Announcement.Content))
                ModelState.AddModelError(nameof(Localizer.RecordAlreadyExists), Localizer.RecordAlreadyExists);

            if (ModelState.IsValid)
            {
                using (IDbContextTransaction contextTransaction = _dataUnitOfWork.BaseUow.AnnouncementsRepository.BeginTransaction())
                {
                    List<int> addedImages = new List<int>();
                    List<int> addedDocs = new List<int>();
                    Announcement announcement = new Announcement();
                    try
                    {
                        announcement = model.Announcement;
                        announcement.UserId = LoggedUserIds.UserId;
                        announcement.InstitutionId = LoggedUserIds.InstitutionId;
                        announcement.OrganizationId = LoggedUserIds.OrganizationId;
                        announcement.CreatedDate = DateTime.Now;
                        announcement.ModifiedDate = DateTime.Now;

                        _dataUnitOfWork.BaseUow.AnnouncementsRepository.Add(announcement);
                        _dataUnitOfWork.BaseUow.AnnouncementsRepository.SaveChanges();

                        //add images and documents
                        foreach (var img in model.AnnouncementPhotos)
                        {
                            AnnouncementPhoto image = new AnnouncementPhoto
                            {
                                StreamId = new Guid(img.Name.Split(".")[0]),
                                AnnouncementId = announcement.Id,
                                Name = sessionImages[img.Name],
                                Size = img.Size,
                                Type = img.Type,
                                Photo = img.Photo,
                                Thumbnail = _imager.GetThumbnail(img.Photo, 120, 120),
                                IsDeleted = false
                            };

                            _dataUnitOfWork.BaseUow.AnnouncementPhotosRepository.Add(image);
                            _dataUnitOfWork.BaseUow.AnnouncementPhotosRepository.SaveChanges();

                            addedImages.Add(image.Id);
                        }
                        foreach (var document in model.AnnouncementDocuments)
                        {
                            AnnouncementDocument doc = new AnnouncementDocument
                            {
                                StreamId = new Guid(document.Name.Split(".")[0]),
                                Document = document.Document,
                                Name = sessionDocuments[document.Name],
                                Size = document.Size,
                                Type = document.Type,
                                AnnouncementId = announcement.Id,
                                IsDeleted = false
                            };

                            _dataUnitOfWork.BaseUow.AnnouncementDocumentsRepository.Add(doc);
                            _dataUnitOfWork.BaseUow.AnnouncementDocumentsRepository.SaveChanges();
                            addedDocs.Add(doc.Id);
                        }

                        if (model.SendArea == "Organizations")
                            _dataUnitOfWork.BaseUow.AnnouncementOrganizationsRepository.AddRange(model.OrganizationIds.Select(oid => new AnnouncementOrganization { AnnouncementId = announcement.Id, OrganizationId = int.Parse(oid) }));
                        else
                            _dataUnitOfWork.BaseUow.AnnouncementUsersRepository.AddRange(model.UserIds.Select(uid => new AnnouncementUser { AnnouncementId = announcement.Id, UserId = int.Parse(uid), Seen = false }));


                        var announcementUser = new AnnouncementUser { AnnouncementId = announcement.Id, UserId = LoggedUserIds.UserId, OccurredAt = DateTime.Now, Seen = true };
                        _dataUnitOfWork.BaseUow.AnnouncementUsersRepository.Add(announcementUser);


                        _dataUnitOfWork.BaseUow.AnnouncementsRepository.SaveChanges();

                        if (model.OrganizationIds != null)
                        {
                            foreach (var item in model.OrganizationIds)
                            {
                                _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.AnnouncementOrganizations, int.Parse(item), GetControllerName(), GetActionName(), null);
                            }
                        }
                        else
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.AnnouncementUsers, announcementUser.Id, GetControllerName(), GetActionName(), null);
                            foreach (var item in model.UserIds)
                            {
                                _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.AnnouncementUsers, int.Parse(item), GetControllerName(), GetActionName(), null);
                            }
                        }
                        foreach (var item in addedImages)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.AnnouncementPhotos, item, GetControllerName(), GetActionName(), null);
                        }
                        foreach (var item in addedDocs)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.AnnouncementDocuments, item, GetControllerName(), GetActionName(), null);
                        }
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.Announcements, announcement.Id, GetControllerName(), GetActionName(), null);

                        _imager.DropzoneCleaner(files);

                        contextTransaction.Commit();
                        return Json(new { success = true, notification = new Notification(NotificationTypes.Success, Localizer.Saved, string.Format(Localizer.SuccessfullySavedAndRecordName, announcement.Title)).ConvertToJson() });
                    }
                    catch (Exception ex)
                    {
                        contextTransaction.Rollback();
                        ModelState.AddModelError(nameof(Localizer.Error), Localizer.Error);
                        _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Add, Tables.Base.Announcements, announcement.Id, GetControllerName(), GetActionName(), ex);
                    }
                }
            }
            return PartialView(MagicStrings.ViewNames._Add, model);
        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(int Id)
        {
            AnnouncementViewModel announcementModel = _dataUnitOfWork.BaseUow.AnnouncementsRepository.GetById(Id);
            AnnouncementOrganizationsViewModel model = new AnnouncementOrganizationsViewModel
            {
                Announcement = announcementModel,
                Photos = _dataUnitOfWork.BaseUow.AnnouncementPhotosRepository.GetPhotosByAnnouncementId(Id),
                Documents = _dataUnitOfWork.BaseUow.AnnouncementDocumentsRepository.GetDocumentsByAnnouncementId(Id)
            };

            IEnumerable<int> organizationIds = _dataUnitOfWork.BaseUow.AnnouncementOrganizationsRepository.OrganizationIdsByAnnouncementId(Id);
            if (organizationIds.Count() > 0)
            {
                model.OrganizationIds = organizationIds.Select(i => i.ToString()).ToArray();
                model.SendToOrganizations = organizationIds.Count() == _dataUnitOfWork.BaseUow.AnnouncementOrganizationsRepository.GetAll().Count() ? "forAllOrganization" : "forCertainOrganization";
                model.SendArea = "Organizations";
            }
            else
            {
                IEnumerable<int> userIds = _dataUnitOfWork.BaseUow.AnnouncementUsersRepository.UserIdsByAnnouncementId(Id);
                model.UserIds = userIds.Select(i => i.ToString()).ToArray();
                model.SendToUsers = userIds.Count() == _dataUnitOfWork.BaseUow.UsersRepository.GetAll().Count() ? "forAllUsers" : "forCertainUsers";
                model.SendArea = "Users";
            }

            var review = _dataUnitOfWork.BaseUow.AnnouncementUsersRepository.GetByUserId(LoggedUserIds.UserId, Id); //announcementUsers
            if (review != null)
            {
                review.OccurredAt = DateTime.Now;
                review.Seen = true;
                _dataUnitOfWork.BaseUow.AnnouncementUsersRepository.Update(review);
                _dataUnitOfWork.BaseUow.AnnouncementUsersRepository.SaveChanges();
                _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.AnnouncementUsers, review.Id, GetControllerName(), GetActionName(), null);
            }
            else
            {
                var announcementUser = new AnnouncementUser { AnnouncementId = Id, UserId = LoggedUserIds.UserId, OccurredAt = DateTime.Now, Seen = true };
                _dataUnitOfWork.BaseUow.AnnouncementUsersRepository.Add(announcementUser);
                _dataUnitOfWork.BaseUow.AnnouncementUsersRepository.SaveChanges();
                _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.AnnouncementUsers, announcementUser.Id, GetControllerName(), GetActionName(), null);
            }
            #region DropzoneSession
            HttpContext.Session.SetObject<Dictionary<string, string>>(SessionKeys.Images, new Dictionary<string, string>());
            HttpContext.Session.SetObject<Dictionary<string, string>>(SessionKeys.Documents, new Dictionary<string, string>());
            HttpContext.Session.SetObject<Dictionary<string, string>>(SessionKeys.DropzoneTempThumbnails, new Dictionary<string, string>());

            model.AnnouncementPhotos = _dataUnitOfWork.BaseUow.AnnouncementPhotosRepository.GetByAnnouncementId(Id).ToList();
            model.AnnouncementDocuments = _dataUnitOfWork.BaseUow.AnnouncementDocumentsRepository.GetByAnnouncementId(Id).ToList();

            var dropzoneThumbs = new Dictionary<string, string>();
            foreach (var file in model.AnnouncementPhotos)
            {
                if (file.Photo.Length > 0)
                {
                    string pathToImages = _environment.WebRootPath + Paths.DropzoneTempThumbnails + file.StreamId.ToString() + System.IO.Path.GetExtension(file.Name);
                    Stream ms = new MemoryStream(_imager.GetThumbnail(file.Photo, 120, 120));
                    using (var stream = new FileStream(pathToImages, FileMode.Create))
                    {
                        await ms.CopyToAsync(stream);
                    }
                    dropzoneThumbs.Add(file.StreamId.ToString() + System.IO.Path.GetExtension(file.Name), file.Name);
                }
            }
            HttpContext.Session.SetObject<Dictionary<string, string>>(SessionKeys.DropzoneTempThumbnails, dropzoneThumbs);

            #endregion
            return PartialView(MagicStrings.ViewNames._Edit, model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(AnnouncementOrganizationsViewModel model)
        {
            #region DropzoneSession
            var sessionDocuments = HttpContext.Session.GetObject<Dictionary<string, string>>(SessionKeys.Documents);
            var sessionImages = HttpContext.Session.GetObject<Dictionary<string, string>>(SessionKeys.Images);

            var files = new List<string>();
            if (sessionImages != null)
            {
                files = sessionImages.Select(x => x.Key).ToList();
            }
            if (sessionDocuments != null)
            {
                files.AddRange(sessionDocuments.Select(x => x.Key).ToList());
            }


            var di = new DirectoryInfo(_environment.WebRootPath + Paths.DropzoneTemp);
            var diThumb = new DirectoryInfo(_environment.WebRootPath + Paths.DropzoneTempThumbnails);
            model.AnnouncementPhotos = _dataUnitOfWork.BaseUow.AnnouncementPhotosRepository.GetByAnnouncementId(model.Announcement.Id).ToList();
            model.AnnouncementDocuments = _dataUnitOfWork.BaseUow.AnnouncementDocumentsRepository.GetByAnnouncementId(model.Announcement.Id).ToList();
            foreach (var formFile in di.GetFiles())
            {
                if (files.Contains(formFile.Name))
                {
                    if (!Data.DocumentFormats.Contains(System.IO.Path.GetExtension(formFile.Extension.ToLower())) && !Data.PhotoFormats.Contains(System.IO.Path.GetExtension(formFile.Extension.ToLower())))
                    {
                        ModelState.AddModelError(nameof(Localizer.UploadTypeFileNotSupported), string.Format(Localizer.UploadTypeFileNotSupported, System.IO.Path.GetExtension(formFile.Extension)));
                        var fileThumb = diThumb.GetFiles().Where(x => x.Name == formFile.Name).FirstOrDefault();
                        if (fileThumb != null)
                            fileThumb.Delete();
                        sessionDocuments.Remove(sessionImages.Where(x => x.Key == formFile.Name).Select(x => x.Key).FirstOrDefault());
                        HttpContext.Session.SetObject<Dictionary<string, string>>(SessionKeys.Documents, sessionImages);
                        formFile.Delete();
                    }
                    else if (Data.PhotoFormats.Contains(System.IO.Path.GetExtension(formFile.Extension)) && !model.AnnouncementPhotos.Select(x => x.Name).Contains(sessionImages.Where(x => x.Key == formFile.Name).Select(x => x.Value).FirstOrDefault()))
                        model.AnnouncementPhotos.Add(new AnnouncementPhoto { AnnouncementId = model.Announcement.Id, Photo = System.IO.File.ReadAllBytes(formFile.FullName), Name = sessionImages[formFile.Name], Size = formFile.Length, Type = formFile.Extension, Thumbnail = _imager.GetThumbnail(System.IO.File.ReadAllBytes(formFile.FullName), 120, 120) });
                    else
                    {
                        if (!model.AnnouncementDocuments.Select(x => x.Name).Contains(sessionDocuments.Where(x => x.Key == formFile.Name).Select(x => x.Value).FirstOrDefault()))
                            model.AnnouncementDocuments.Add(new AnnouncementDocument { AnnouncementId = model.Announcement.Id, Document = System.IO.File.ReadAllBytes(formFile.FullName), Name = sessionDocuments[formFile.Name], Size = formFile.Length, Type = formFile.Extension });
                    }
                }
            }
            #endregion
            if (model.SendArea == "Organizations")
            {
                if (model.SendToOrganizations == "forAllOrganization")
                {
                    model.OrganizationIds = _dataUnitOfWork.BaseUow.AnnouncementOrganizationsRepository.GetAll().Select(i => i.Id.ToString()).ToArray();
                    if (model.OrganizationIds == null)
                    {
                        ModelState.AddModelError(nameof(Localizer.NotFoundAnyOrganizationAsRecipient), Localizer.NotFoundAnyOrganizationAsRecipient);
                    }
                }
                else if (model.OrganizationIds == null)
                {
                    ModelState.AddModelError(nameof(Localizer.ErrorMessageSelectOrganizationAsRecipientMin1), Localizer.ErrorMessageSelectOrganizationAsRecipientMin1);
                }
            }
            else if (model.SendArea == "Users")
            {
                if (model.SendToUsers == "forAllUsers")
                {
                    model.UserIds = _dataUnitOfWork.BaseUow.UsersRepository.GetAll().Where(x => x.Id != LoggedUserIds.UserId).Select(i => i.Id.ToString()).ToArray();
                    if (model.UserIds == null)
                    {
                        ModelState.AddModelError(nameof(Localizer.NotFoundAnyUsersAsRecipient), Localizer.NotFoundAnyUsersAsRecipient);
                    }
                }
                else if (model.UserIds == null)
                {
                    ModelState.AddModelError(nameof(Localizer.ErrorMessageSelectUserAsRecipientMin1), Localizer.ErrorMessageSelectUserAsRecipientMin1);
                }
            }

            if (ModelState.IsValid)
            {
                var addedImages = new List<int>();
                var addedDocs = new List<int>();
                var addedOrganizations = new List<int>();
                var addedAnnouncementUsers = new List<int>();
                var removedOrganizations = new List<int>();
                var removedAnnouncementUsers = new List<int>();
                Announcement announcement = new Announcement();
                using (IDbContextTransaction contextTransaction = _dataUnitOfWork.BaseUow.AnnouncementsRepository.BeginTransaction())
                {
                    try
                    {
                        announcement = model.Announcement;
                        announcement.ModifiedDate = DateTime.Now;
                        _dataUnitOfWork.BaseUow.AnnouncementsRepository.Update(announcement);


                        //add images and documents
                        foreach (var formFile in di.GetFiles())
                        {
                            if (files.Contains(formFile.Name))
                            {
                                string extension = System.IO.Path.GetExtension(formFile.Extension);
                                if (Data.PhotoFormats.Contains(extension))
                                {
                                    AnnouncementPhoto image = new AnnouncementPhoto
                                    {
                                        StreamId = new Guid(formFile.Name.Split(".")[0]),
                                        AnnouncementId = announcement.Id,
                                        Name = sessionImages[formFile.Name],
                                        Size = formFile.Length,
                                        Type = extension,
                                        Photo = System.IO.File.ReadAllBytes(formFile.FullName)
                                    };

                                    image.Thumbnail = _imager.GetThumbnail(image.Photo, 120, 120);

                                    _dataUnitOfWork.BaseUow.AnnouncementPhotosRepository.Add(image);
                                    _dataUnitOfWork.BaseUow.AnnouncementPhotosRepository.SaveChanges();
                                    addedImages.Add(image.Id);
                                }
                                else if (Data.DocumentFormats.Contains(extension))
                                {
                                    AnnouncementDocument doc = new AnnouncementDocument
                                    {
                                        StreamId = new Guid(formFile.Name.Split(".")[0]),
                                        Document = System.IO.File.ReadAllBytes(formFile.FullName),
                                        Name = sessionDocuments[formFile.Name],
                                        Size = formFile.Length,
                                        Type = extension,
                                        AnnouncementId = announcement.Id
                                    };

                                    _dataUnitOfWork.BaseUow.AnnouncementDocumentsRepository.Add(doc);
                                    _dataUnitOfWork.BaseUow.AnnouncementDocumentsRepository.SaveChanges();
                                    addedDocs.Add(doc.Id);
                                }
                            }
                        }

                        if (model.SendArea == "Organizations")
                        {
                            IEnumerable<int> oldAnnouncementOrganizations = _dataUnitOfWork.BaseUow.AnnouncementOrganizationsRepository.OrganizationIdsByAnnouncementId(announcement.Id);
                            foreach (var item in model.OrganizationIds)
                            {
                                if (!oldAnnouncementOrganizations.Contains(int.Parse(item)))
                                {
                                    var announcementOrganization = new AnnouncementOrganization { AnnouncementId = announcement.Id, OrganizationId = int.Parse(item) };
                                    _dataUnitOfWork.BaseUow.AnnouncementOrganizationsRepository.Add(announcementOrganization);
                                    _dataUnitOfWork.BaseUow.AnnouncementOrganizationsRepository.SaveChanges();
                                    addedOrganizations.Add(announcementOrganization.Id);
                                }
                            }
                            int[] organizations = model.OrganizationIds.Select(int.Parse).ToArray();
                            foreach (var item in oldAnnouncementOrganizations)
                            {
                                if (!organizations.Contains(item))
                                {
                                    AnnouncementOrganization announcementOrganization = _dataUnitOfWork.BaseUow.AnnouncementOrganizationsRepository.GetByOrganizationId(item, announcement.Id);
                                    _dataUnitOfWork.BaseUow.AnnouncementOrganizationsRepository.Remove(announcementOrganization);
                                    _dataUnitOfWork.BaseUow.AnnouncementOrganizationsRepository.SaveChanges();
                                    removedOrganizations.Add(announcementOrganization.Id);
                                }
                            }
                            removedAnnouncementUsers = _dataUnitOfWork.BaseUow.AnnouncementUsersRepository.RemoveWithoutCreator(announcement.Id, LoggedUserIds.UserId);
                        }
                        else
                        {
                            IEnumerable<int> oldAnnouncementUsers = _dataUnitOfWork.BaseUow.AnnouncementUsersRepository.UserIdsByAnnouncementId(announcement.Id);
                            foreach (var item in model.UserIds)
                            {
                                if (!oldAnnouncementUsers.Contains(int.Parse(item)))
                                {
                                    var announcementUser = new AnnouncementUser { AnnouncementId = announcement.Id, UserId = int.Parse(item) };
                                    _dataUnitOfWork.BaseUow.AnnouncementUsersRepository.Add(announcementUser);
                                    _dataUnitOfWork.BaseUow.AnnouncementUsersRepository.SaveChanges();
                                    addedAnnouncementUsers.Add(announcementUser.Id);
                                }
                            }
                            int[] users = model.UserIds.Where(x => int.Parse(x) != LoggedUserIds.UserId).Select(int.Parse).ToArray();
                            foreach (var item in oldAnnouncementUsers)
                            {
                                if (!users.Contains(item) && item != LoggedUserIds.UserId)
                                {
                                    AnnouncementUser announcementUser = _dataUnitOfWork.BaseUow.AnnouncementUsersRepository.GetByUserId(item, announcement.Id);
                                    _dataUnitOfWork.BaseUow.AnnouncementUsersRepository.Remove(announcementUser);
                                    _dataUnitOfWork.BaseUow.AnnouncementUsersRepository.SaveChanges();
                                    removedAnnouncementUsers.Add(announcementUser.Id);
                                }
                            }
                            removedOrganizations = _dataUnitOfWork.BaseUow.AnnouncementOrganizationsRepository.Remove(announcement.Id);
                        }


                        var reviews = _dataUnitOfWork.BaseUow.AnnouncementUsersRepository.GetAnnouncementUsers(LoggedUserIds.UserId, announcement.Id).ToList();
                        _dataUnitOfWork.BaseUow.AnnouncementUsersRepository.SetAnnouncementReviews(LoggedUserIds.UserId, announcement.Id, false);  // puca

                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.Announcements, model.Announcement.Id, GetControllerName(), GetActionName(), null);
                        foreach (var item in reviews)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.AnnouncementUsers, item.Id, GetControllerName(), GetActionName(), null);
                        }
                        foreach (var item in addedImages)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.AnnouncementPhotos, item, GetControllerName(), GetActionName(), null);
                        }
                        foreach (var item in addedDocs)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.AnnouncementDocuments, item, GetControllerName(), GetActionName(), null);
                        }
                        foreach (var item in addedOrganizations)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.AnnouncementOrganizations, item, GetControllerName(), GetActionName(), null);
                        }
                        foreach (var item in removedOrganizations)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.AnnouncementOrganizations, item, GetControllerName(), GetActionName(), null);
                        }
                        foreach (var item in addedAnnouncementUsers)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.AnnouncementUsers, item, GetControllerName(), GetActionName(), null);
                        }
                        foreach (var item in removedAnnouncementUsers)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.AnnouncementUsers, item, GetControllerName(), GetActionName(), null);
                        }

                        _imager.DropzoneCleaner(files);
                        _dataUnitOfWork.BaseUow.AnnouncementsRepository.SaveChanges();
                        contextTransaction.Commit();
                        return Json(new { success = true, notification = new Notification(NotificationTypes.Success, _localizer.Saved, string.Format(_localizer.SuccessfullySavedAndRecordName, model.Announcement.Title)).ConvertToJson() });
                    }
                    catch (Exception ex)
                    {
                        contextTransaction.Rollback();
                        _imager.DropzoneCleaner();
                        ModelState.AddModelError(nameof(_localizer.Error), _localizer.Error);

                        _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Edit, Tables.Base.Announcements, model.Announcement.Id, GetControllerName(), GetActionName(), ex);
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
            using (IDbContextTransaction contextTransaction = _dataUnitOfWork.BaseUow.AnnouncementsRepository.BeginTransaction())
            {
                try
                {
                    var model = _dataUnitOfWork.BaseUow.AnnouncementsRepository.GetById(Id);
                    var removedOrganizations = _dataUnitOfWork.BaseUow.AnnouncementOrganizationsRepository.Remove(Id);
                    var removedAnnouncementUsers = _dataUnitOfWork.BaseUow.AnnouncementUsersRepository.Remove(Id);
                    var removedPhotos = _dataUnitOfWork.BaseUow.AnnouncementPhotosRepository.Remove(Id);
                    var removedDocuments = _dataUnitOfWork.BaseUow.AnnouncementDocumentsRepository.Remove(Id);
                    _dataUnitOfWork.BaseUow.AnnouncementsRepository.Remove(model);

                    _dataUnitOfWork.BaseUow.AnnouncementUsersRepository.SaveChanges();

                    contextTransaction.Commit();
                    foreach (var item in removedOrganizations)
                    {
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.AnnouncementOrganizations, item, GetControllerName(), GetActionName(), null);
                    }
                    foreach (var item in removedDocuments)
                    {
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.AnnouncementDocuments, item, GetControllerName(), GetActionName(), null);
                    }
                    foreach (var item in removedPhotos)
                    {
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.AnnouncementPhotos, item, GetControllerName(), GetActionName(), null);
                    }
                    foreach (var item in removedAnnouncementUsers)
                    {
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.AnnouncementUsers, item, GetControllerName(), GetActionName(), null);
                    }
                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.Announcements, model.Id, GetControllerName(), GetActionName(), null);
                    notification = new Notification(NotificationTypes.Success, _localizer.Removed, string.Format(_localizer.SuccessfullyRemovedName, model.Title));
                }
                catch (Exception ex)
                {
                    contextTransaction.Rollback();
                    notification = new Notification(NotificationTypes.Error, Localizer.ErrorFriendly, Localizer.AnErrorOccurredFriendly);
                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.Announcements, Id, GetControllerName(), GetActionName(), ex);
                }
            }
            return Json(notification.ConvertToJson());

        }
        public JsonResult DeleteAll(List<int> list)
        {
            var removedAnnouncements = new List<int>();
            var removedAnnouncementOrganizations = new List<List<int>>();
            var removedAnnouncementUsers = new List<List<int>>();
            var removedAnnouncementPhotos = new List<List<int>>();
            var removedAnnouncementDocuments = new List<List<int>>();
            var removedAnnouncementUserReviews = new List<List<int>>();

            Notification notification = null;
            using (IDbContextTransaction contextTransaction = _dataUnitOfWork.BaseUow.AnnouncementsRepository.BeginTransaction())
            {
                try
                {
                    foreach (var item in list)
                    {
                        removedAnnouncements.Add(_dataUnitOfWork.BaseUow.AnnouncementsRepository.Remove(item));
                        removedAnnouncementOrganizations.Add(_dataUnitOfWork.BaseUow.AnnouncementOrganizationsRepository.Remove(item));
                        removedAnnouncementUsers.Add(_dataUnitOfWork.BaseUow.AnnouncementUsersRepository.Remove(item));
                        removedAnnouncementPhotos.Add(_dataUnitOfWork.BaseUow.AnnouncementPhotosRepository.Remove(item));
                        removedAnnouncementDocuments.Add(_dataUnitOfWork.BaseUow.AnnouncementDocumentsRepository.Remove(item));
                    }
                    _dataUnitOfWork.BaseUow.AnnouncementOrganizationsRepository.SaveChanges();
                    _dataUnitOfWork.BaseUow.AnnouncementPhotosRepository.SaveChanges();
                    _dataUnitOfWork.BaseUow.AnnouncementDocumentsRepository.SaveChanges();
                    _dataUnitOfWork.BaseUow.AnnouncementsRepository.SaveChanges();
                    _dataUnitOfWork.BaseUow.AnnouncementUsersRepository.SaveChanges();

                    contextTransaction.Commit();

                    #region Logging
                    foreach (var item in removedAnnouncements)
                    {
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.Announcements, item, GetControllerName(), GetActionName(), null);
                    }
                    foreach (var item in removedAnnouncementUsers)
                    {
                        foreach (var item1 in item)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.AnnouncementUsers, item1, GetControllerName(), GetActionName(), null);
                        }
                    }
                    foreach (var item in removedAnnouncementOrganizations)
                    {
                        foreach (var item1 in item)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.AnnouncementOrganizations, item1, GetControllerName(), GetActionName(), null);
                        }
                    }
                    foreach (var item in removedAnnouncementPhotos)
                    {
                        foreach (var item1 in item)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.AnnouncementPhotos, item1, GetControllerName(), GetActionName(), null);
                        }
                    }
                    foreach (var item in removedAnnouncementDocuments)
                    {
                        foreach (var item1 in item)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.AnnouncementDocuments, item1, GetControllerName(), GetActionName(), null);
                        }
                    }
                    #endregion

                    notification = new Notification(NotificationTypes.Success, Localizer.Removed, string.Format(Localizer.SuccessfullyRemovedName, Localizer.Announcements));
                }
                catch (Exception ex)
                {
                    contextTransaction.Rollback();
                    notification = new Notification(NotificationTypes.Error, Localizer.ErrorFriendly, Localizer.AnErrorOccurredFriendly);
                    foreach (var item in list)
                    {
                        _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.Announcements, item, GetControllerName(), GetActionName(), ex);
                    }
                }
            }
            return Json(notification.ConvertToJson());

        }
        public JsonResult DeleteImage(int Id)
        {
            Notification notification = null;
            try
            {
                AnnouncementPhoto announcementPhoto = _dataUnitOfWork.BaseUow.AnnouncementPhotosRepository.GetById(Id);
                _dataUnitOfWork.BaseUow.AnnouncementPhotosRepository.Remove(announcementPhoto);
                _dataUnitOfWork.BaseUow.AnnouncementPhotosRepository.SaveChanges();
                _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.Announcements, Id, GetControllerName(), GetActionName(), null);
                notification = new Notification(NotificationTypes.Success, _localizer.Removed, string.Format(_localizer.SuccessfullyRemovedName, announcementPhoto.Name));
            }
            catch (Exception ex)
            {
                notification = new Notification(NotificationTypes.Error, Localizer.ErrorFriendly, Localizer.AnErrorOccurredFriendly);
                _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.AnnouncementPhotos, Id, GetControllerName(), GetActionName(), ex);
            }
            return Json(notification.ConvertToJson());
        }
        public JsonResult DeleteDocument(int Id)
        {
            Notification notification = null;
            try
            {
                AnnouncementDocument announcementDocument = _dataUnitOfWork.BaseUow.AnnouncementDocumentsRepository.GetById(Id);
                _dataUnitOfWork.BaseUow.AnnouncementDocumentsRepository.Remove(announcementDocument);
                _dataUnitOfWork.BaseUow.AnnouncementDocumentsRepository.SaveChanges();
                _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.Announcements, Id, GetControllerName(), GetActionName(), null);
                notification = new Notification(NotificationTypes.Success, _localizer.Removed, string.Format(_localizer.SuccessfullyRemovedName, announcementDocument.Name));
            }
            catch (Exception ex)
            {
                notification = new Notification(NotificationTypes.Error, Localizer.ErrorFriendly, Localizer.AnErrorOccurredFriendly);
                _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.AnnouncementDocuments, Id, GetControllerName(), GetActionName(), ex);
            }
            return Json(notification.ConvertToJson());
        }

        #endregion

        #region Preview
        public IActionResult Preview(int Id)
        {
            AnnouncementViewModel announcementModel = _dataUnitOfWork.BaseUow.AnnouncementsRepository.GetById(Id);
            AnnouncementOrganizationsViewModel model = new AnnouncementOrganizationsViewModel
            {
                Announcement = announcementModel,
                Documents = _dataUnitOfWork.BaseUow.AnnouncementDocumentsRepository.GetDocumentsByAnnouncementId(Id),
                SrcFiles = new Dictionary<string, string>()
            };

            List<AnnouncementPhoto> byteFiles = new List<AnnouncementPhoto>(_dataUnitOfWork.BaseUow.AnnouncementPhotosRepository.GetPhotosByAnnouncementId(Id).ToList());
            foreach (var item in byteFiles)
            {
                var base64Thumbnail = Convert.ToBase64String(item.Thumbnail);
                var base64Image = Convert.ToBase64String(item.Photo);
                model.SrcFiles[string.Format("data:image/png|jpg;base64,{0}", base64Image)] = (string.Format("data:image/png|jpg;base64,{0}", base64Thumbnail));
            }

            var review = _dataUnitOfWork.BaseUow.AnnouncementUsersRepository.GetByUserId(LoggedUserIds.UserId, Id);
            if (review != null)
            {
                review.OccurredAt = DateTime.Now;
                review.Seen = true;
                _dataUnitOfWork.BaseUow.AnnouncementUsersRepository.Update(review);
                _dataUnitOfWork.BaseUow.AnnouncementUsersRepository.SaveChanges();
                _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.AnnouncementUsers, review.Id, GetControllerName(), GetActionName(), null);
            }
            else
            {
                var announcementUser = new AnnouncementUser { AnnouncementId = Id, UserId = LoggedUserIds.UserId, OccurredAt = DateTime.Now, Seen = true };
                _dataUnitOfWork.BaseUow.AnnouncementUsersRepository.Add(announcementUser);
                _dataUnitOfWork.BaseUow.AnnouncementUsersRepository.SaveChanges();
                _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.AnnouncementUsers, announcementUser.Id, GetControllerName(), GetActionName(), null);
            }
            return PartialView(MagicStrings.ViewNames._Preview, model);
        }
        [HttpGet]
        public JsonResult JsonDocuments(int Id)
        {
            IEnumerable<AnnouncementDocument> announcementDocuments = _dataUnitOfWork.BaseUow.AnnouncementDocumentsRepository.GetDocumentsByAnnouncementId(Id);

            var announcementDocumentsJson = JsonConvert.SerializeObject(announcementDocuments);

            return new JsonResult(announcementDocumentsJson);
        }
        #endregion

        #region DropzoneUpload
        [HttpPost]
        [IgnoreAntiforgeryToken]
        //[Authorization(Enumerations.Functionalities.Add)]
        public IActionResult Upload()
        {
            var files = HttpContext.Request.Form.Files;
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    if (Data.PhotoFormats.Contains(Path.GetExtension(file.FileName).ToLower()))
                    {
                        var images = HttpContext.Session.GetObject<Dictionary<string, string>>(SessionKeys.Images);
                        images.Add(_imager.UploadDropzoneImage(file), file.FileName);
                        HttpContext.Session.SetObject(SessionKeys.Images, images);
                    }
                    else
                    {
                        var documents = HttpContext.Session.GetObject<Dictionary<string, string>>(SessionKeys.Documents);
                        documents.Add(_imager.UploadDropzoneDocument(file), file.FileName);
                        HttpContext.Session.SetObject<Dictionary<string, string>>(SessionKeys.Documents, documents);
                    }
                }
            }
            return Ok();
        }

        public IActionResult RemoveFile(int announcementId, string fileName)
        {

            Notification notification = null;
            var di = new DirectoryInfo(_environment.WebRootPath + Paths.DropzoneTemp);
            var diThumb = new DirectoryInfo(_environment.WebRootPath + Paths.DropzoneTempThumbnails);
            try
            {
                if (Data.DocumentFormats.Contains(System.IO.Path.GetExtension(fileName).ToLower()))
                {
                    var sessionDocuments = HttpContext.Session.GetObject<Dictionary<string, string>>(SessionKeys.Documents);
                    var streamId = fileName.Length > 32 ? new Guid(fileName.Split(".")[0]) : default(Guid);
                    var document = _dataUnitOfWork.BaseUow.AnnouncementDocumentsRepository.GetByStreamId(streamId, announcementId);
                    if (document != null)
                    {
                        _dataUnitOfWork.BaseUow.AnnouncementDocumentsRepository.Remove(document);
                        _dataUnitOfWork.BaseUow.AnnouncementDocumentsRepository.SaveChanges();
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.EventDocuments, document.Id, GetControllerName(), GetActionName(), null);
                        notification = new Notification(NotificationTypes.Success, _localizer.Removed, string.Format(_localizer.SuccessfullyRemovedName, document.Name));
                    }
                    else
                    {
                        if (sessionDocuments.Where(x => x.Value == fileName).Select(x => x.Key).FirstOrDefault() != null)
                        {
                            fileName = sessionDocuments.Where(x => x.Value == fileName).Select(x => x.Key).FirstOrDefault();
                        }
                        foreach (var item in di.GetFiles())
                        {
                            if (item.Name == fileName)
                            {
                                item.Delete();
                                var sessionDoc = sessionDocuments.Where(x => x.Key == fileName).FirstOrDefault();
                                if (sessionDoc.Key != null)
                                {
                                    sessionDocuments.Remove(sessionDoc.Key);
                                    HttpContext.Session.SetObject<Dictionary<string, string>>(SessionKeys.Documents, sessionDocuments);
                                }
                                notification = new Notification(NotificationTypes.Success, _localizer.Removed, string.Format(_localizer.SuccessfullyRemovedName, sessionDoc.Value));
                                break;
                            }

                        }
                    }
                }
                else
                {
                    var sessionImages = HttpContext.Session.GetObject<Dictionary<string, string>>(SessionKeys.Images);
                    var existsThumbs = HttpContext.Session.GetObject<Dictionary<string, string>>(SessionKeys.DropzoneTempThumbnails);
                    var imageName = string.Empty;

                    var image = _dataUnitOfWork.BaseUow.AnnouncementPhotosRepository.GetByName(fileName, announcementId);
                    if (image == null && sessionImages.Where(x => x.Value == fileName).Select(x => x.Key).FirstOrDefault() != null)
                    {
                        fileName = sessionImages.Where(x => x.Value == fileName).Select(x => x.Key).FirstOrDefault();
                    }
                    foreach (var fileThumb in diThumb.GetFiles())
                    {
                        if (fileThumb.Name == fileName)
                        {
                            fileThumb.Delete();
                            var file = di.GetFiles().Where(x => x.Name == fileThumb.Name).FirstOrDefault();
                            if (file != null)
                                file.Delete();

                            if (image == null)
                            {
                                image = _dataUnitOfWork.BaseUow.AnnouncementPhotosRepository.GetByName(existsThumbs.Where(x => x.Key == fileName).Select(x => x.Value).FirstOrDefault(), announcementId);
                            }
                            if (image != null)
                            {
                                _dataUnitOfWork.BaseUow.AnnouncementPhotosRepository.Remove(image);
                                _dataUnitOfWork.BaseUow.AnnouncementPhotosRepository.SaveChanges();
                                _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.EventImages, image.Id, GetControllerName(), GetActionName(), null);
                            }
                            var sessionImg = sessionImages.Where(x => x.Key == fileName).FirstOrDefault();
                            if (sessionImg.Key != null)
                            {
                                imageName = sessionImg.Value;
                                sessionImages.Remove(sessionImg.Key);
                                HttpContext.Session.SetObject<Dictionary<string, string>>(SessionKeys.Images, sessionImages);
                            }
                            else
                            {
                                var sessionThumb = existsThumbs.Where(x => x.Key == fileName).FirstOrDefault();
                                if (sessionThumb.Key != null)
                                {
                                    imageName = sessionThumb.Value;
                                    existsThumbs.Remove(sessionThumb.Key);
                                    HttpContext.Session.SetObject<Dictionary<string, string>>(SessionKeys.DropzoneTempThumbnails, existsThumbs);
                                }
                            }
                            notification = new Notification(NotificationTypes.Success, _localizer.Removed, string.Format(_localizer.SuccessfullyRemovedName, imageName));
                            break;
                        }
                    }
                }
                return Json(notification.ConvertToJson());
            }
            catch (Exception ex)
            {
                notification = new Notification(NotificationTypes.Error, _localizer.Error, _localizer.ErrorFriendly);
            }
            return Json(notification.ConvertToJson());
        }

        #endregion
    }
}