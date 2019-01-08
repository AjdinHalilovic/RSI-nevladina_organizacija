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
using nevladinaOrg.Web.Helpers.GoogleMapsHelper;
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
using Authorization = nevladinaOrg.Web.Helpers.Authorization;

namespace nevladinaOrg.Web.Areas.Administration.Controllers
{
    [Area(MagicStrings.AreaNames.Administration)]
    [Authorization]
    public class EventsController : BaseController
    {
        #region Properties
        IDataUnitOfWork _dataUnitOfWork;

        private IHostingEnvironment _hostingEnvironment;
        private Localizer _localizer;
        private ILogger _logger;
        private GoogleMapsApiHelper _googleMapsApi;
        private IImageHelper _imageHelper;

        #endregion

        public EventsController(IDataUnitOfWork dataUnitOfWork,
                                 IHostingEnvironment hostingEnvironment,
                                Localizer localizer, ILogger logger,
                                GoogleMapsApiHelper googleMapsApi, IImageHelper imageHelper,
                                IBreadcrumb breadcrumb, IStringLocalizerFactory stringLocalizerFactory)
        : base(breadcrumb, stringLocalizerFactory)
        {
            _dataUnitOfWork = dataUnitOfWork;

            _hostingEnvironment = hostingEnvironment;
            _localizer = localizer;
            _logger = logger;
            _googleMapsApi = googleMapsApi;
            _imageHelper = imageHelper;
        }

        #region Index
        [HttpGet]
        public JsonResult JsonIndex()
        {
            IEnumerable<EventDTO> events = _dataUnitOfWork.BaseUow.EventsDTORepository.GetAll();

            var eventsJson = JsonConvert.SerializeObject(events);

            return new JsonResult(eventsJson);
        }
        [Authorization]
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region Add
        [HttpGet]
        public async Task<IActionResult> Add(Enumerations.SaveAndOptions option)
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
            _imageHelper.DropzoneCleaner(files);

            #endregion

            var model = new EventViewModel
            {
                EventDocuments = new List<EventDocument>(),
                EventImages = new List<EventImage>(),
                SaveAndOptions = option
            };

            return PartialView(MagicStrings.ViewNames._Add, model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(EventViewModel model)
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


            var di = new DirectoryInfo(_hostingEnvironment.WebRootPath + Paths.DropzoneTemp);
            var diThumb = new DirectoryInfo(_hostingEnvironment.WebRootPath + Paths.DropzoneTempThumbnails);
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
                    else if (Data.PhotoFormats.Contains(System.IO.Path.GetExtension(formFile.Extension)) && !model.EventImages.Select(x => x.Name).Contains(formFile.Name))
                        model.EventImages.Add(new EventImage { EventId = model.Id, Image = System.IO.File.ReadAllBytes(formFile.FullName), Name = formFile.Name, Size = formFile.Length, Type = formFile.Extension });
                    else
                    {
                        if (!model.EventDocuments.Select(x => x.Name).Contains(formFile.Name))
                            model.EventDocuments.Add(new EventDocument { EventId = model.Id, Document = System.IO.File.ReadAllBytes(formFile.FullName), Name = formFile.Name, Size = formFile.Length, Type = formFile.Extension });
                    }
                }
            }
            #endregion

            if (model.DateFrom > model.DateTo)
                ModelState.AddModelError(nameof(Localizer.ErrorMessageDateFromBeforeDateTo), Localizer.ErrorMessageDateFromBeforeDateTo);

            if (!ModelState.IsValid)
                return PartialView(MagicStrings.ViewNames._Add, model);

            if (_dataUnitOfWork.BaseUow.EventsRepository.GetExists(model.Name))
                ModelState.AddModelError(nameof(Localizer.RecordAlreadyExists), Localizer.RecordAlreadyExists);
            using (IDbContextTransaction contextTransaction = _dataUnitOfWork.BaseUow.EventsRepository.BeginTransaction())
            {
                if (ModelState.IsValid)
                {
                    var _event = new Event();
                    var addedImages = new List<int>();
                    var addedDocs = new List<int>();
                    try
                    {
                        _event = model;
                        _event.InstitutionOrganizerId = LoggedUserIds.InstitutionId;
                        _event.OrganizationOrganizerId = LoggedUserIds.OrganizationId;
                        _dataUnitOfWork.BaseUow.EventsRepository.Add(_event);
                        _dataUnitOfWork.BaseUow.EventsRepository.SaveChanges();

                        var eventUser = new EventUser { EventId = _event.Id, UserId = LoggedUserIds.UserId, OccurredAt = DateTime.Now, Seen = true };
                        _dataUnitOfWork.BaseUow.EventUsersRepository.Add(eventUser);
                        _dataUnitOfWork.BaseUow.EventUsersRepository.SaveChanges();

                        //add images and documents
                        foreach (var img in model.EventImages)
                        {
                            EventImage image = new EventImage
                            {
                                StreamId = new Guid(img.Name.Split(".")[0]),
                                EventId = _event.Id,
                                Name = sessionImages[img.Name],
                                Size = img.Size,
                                Type = img.Type,
                                Image = img.Image,
                                IsDeleted = false
                            };

                            _dataUnitOfWork.BaseUow.EventImagesRepository.Add(image);
                            _dataUnitOfWork.BaseUow.EventImagesRepository.SaveChanges();
                            addedImages.Add(image.Id);
                        }
                        foreach (var document in model.EventDocuments)
                        {
                            EventDocument doc = new EventDocument
                            {
                                StreamId = new Guid(document.Name.Split(".")[0]),
                                Document = document.Document,
                                Name = sessionDocuments[document.Name],
                                Size = document.Size,
                                Type = document.Type,
                                EventId = _event.Id,
                                IsDeleted = false
                            };

                            _dataUnitOfWork.BaseUow.EventDocumentsRepository.Add(doc);
                            _dataUnitOfWork.BaseUow.EventDocumentsRepository.SaveChanges();
                            addedDocs.Add(doc.Id);
                        }

                        _imageHelper.DropzoneCleaner(files);
                        contextTransaction.Commit();

                        foreach (var item in addedImages)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.EventImages, item, GetControllerName(), GetActionName(), null);
                        }
                        foreach (var item in addedDocs)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.EventDocuments, item, GetControllerName(), GetActionName(), null);
                        }
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.Events, _event.Id, GetControllerName(), GetActionName(), null);
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.EventUsers, eventUser.Id, GetControllerName(), GetActionName(), null);


                        var newRowData = _dataUnitOfWork.BaseUow.EventsDTORepository.GetByEventId(_event.Id); // poraditi na appendu datatable row-a
                        return Json(new { success = true, notification = new Notification(NotificationTypes.Success, Localizer.Saved, string.Format(Localizer.SuccessfullySavedAndRecordName, _event.Name)).ConvertToJson()/*,newData= JsonConvert.SerializeObject(newRowData)*/ });
                    }
                    catch (Exception ex)
                    {
                        contextTransaction.Rollback();
                        _imageHelper.DropzoneCleaner(files);

                        ModelState.AddModelError(nameof(Localizer.Error), Localizer.Error);

                        _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Add, Tables.Base.Events, _event.Id, GetControllerName(), GetActionName(), ex);
                    }
                }
            }
            return PartialView(MagicStrings.ViewNames._Add, model);
        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(int Id)
        {
            HttpContext.Session.SetObject<Dictionary<string, string>>(SessionKeys.Images, new Dictionary<string, string>());
            HttpContext.Session.SetObject<Dictionary<string, string>>(SessionKeys.Documents, new Dictionary<string, string>());
            HttpContext.Session.SetObject<Dictionary<string, string>>(SessionKeys.DropzoneTempThumbnails, new Dictionary<string, string>());

            EventViewModel model = _dataUnitOfWork.BaseUow.EventsRepository.GetById(Id);
            model.EventImages = _dataUnitOfWork.BaseUow.EventImagesRepository.GetByEventId(Id).ToList();
            model.EventDocuments = _dataUnitOfWork.BaseUow.EventDocumentsRepository.GetByEventId(Id).ToList();

            var dropzoneThumbs = new Dictionary<string, string>();
            foreach (var file in model.EventImages)
            {
                if (file.Image.Length > 0)
                {
                    string pathToImages = _hostingEnvironment.WebRootPath + Paths.DropzoneTempThumbnails + file.StreamId.ToString() + System.IO.Path.GetExtension(file.Name);
                    Stream ms = new MemoryStream(_imageHelper.GetThumbnail(file.Image, 120, 120));
                    using (var stream = new FileStream(pathToImages, FileMode.Create))
                    {
                        await ms.CopyToAsync(stream);
                    }
                    dropzoneThumbs.Add(file.StreamId.ToString() + System.IO.Path.GetExtension(file.Name), file.Name);
                }
            }
            HttpContext.Session.SetObject<Dictionary<string, string>>(SessionKeys.DropzoneTempThumbnails, dropzoneThumbs);

            var review = _dataUnitOfWork.BaseUow.EventUsersRepository.GetReview(LoggedUserIds.UserId, Id);
            if (review != null)
            {
                review.OccurredAt = DateTime.Now;
                review.Seen = true;
                _dataUnitOfWork.BaseUow.EventUsersRepository.Update(review);
                _dataUnitOfWork.BaseUow.EventUsersRepository.SaveChanges();
                _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.EventUsers, review.Id, GetControllerName(), GetActionName(), null);
            }
            else
            {
                var eventUser = new EventUser { EventId = Id, UserId = LoggedUserIds.UserId, OccurredAt = DateTime.Now, Seen = true };
                _dataUnitOfWork.BaseUow.EventUsersRepository.Add(eventUser);
                _dataUnitOfWork.BaseUow.EventUsersRepository.SaveChanges();
                _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.EventUsers, eventUser.Id, GetControllerName(), GetActionName(), null);
            }

            return PartialView(MagicStrings.ViewNames._Edit, model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(EventViewModel model)
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


            var di = new DirectoryInfo(_hostingEnvironment.WebRootPath + Paths.DropzoneTemp);
            var diThumb = new DirectoryInfo(_hostingEnvironment.WebRootPath + Paths.DropzoneTempThumbnails);
            model.EventImages = _dataUnitOfWork.BaseUow.EventImagesRepository.GetByEventId(model.Id).ToList();
            model.EventDocuments = _dataUnitOfWork.BaseUow.EventDocumentsRepository.GetByEventId(model.Id).ToList();
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
                    else if (Data.PhotoFormats.Contains(System.IO.Path.GetExtension(formFile.Extension)) && !model.EventImages.Select(x => x.Name).Contains(sessionImages.Where(x => x.Key == formFile.Name).Select(x => x.Value).FirstOrDefault()))
                        model.EventImages.Add(new EventImage { EventId = model.Id, Image = System.IO.File.ReadAllBytes(formFile.FullName), Name = sessionImages[formFile.Name], Size = formFile.Length, Type = formFile.Extension });
                    else
                    {
                        if (!model.EventDocuments.Select(x => x.Name).Contains(sessionDocuments.Where(x => x.Key == formFile.Name).Select(x => x.Value).FirstOrDefault()))
                            model.EventDocuments.Add(new EventDocument { EventId = model.Id, Document = System.IO.File.ReadAllBytes(formFile.FullName), Name = sessionDocuments[formFile.Name], Size = formFile.Length, Type = formFile.Extension });
                    }
                }
            }
            #endregion


            if (model.DateFrom > model.DateTo)
                ModelState.AddModelError(nameof(Localizer.ErrorMessageDateFromBeforeDateTo), Localizer.ErrorMessageDateFromBeforeDateTo);

            if (!ModelState.IsValid)
                return PartialView(MagicStrings.ViewNames._Edit, model);
            using (IDbContextTransaction context = _dataUnitOfWork.BaseUow.EventsRepository.BeginTransaction())
            {
                try
                {
                    Event _event = new Event();
                    var addedimages = new List<int>();
                    var addeddocs = new List<int>();

                    _event = model;
                    _dataUnitOfWork.BaseUow.EventsRepository.Update(_event);
                    _dataUnitOfWork.BaseUow.EventsRepository.SaveChanges();

                    //add images and documents
                    foreach (var formFile in di.GetFiles())
                    {
                        if (files.Contains(formFile.Name))
                        {
                            string extension = System.IO.Path.GetExtension(formFile.Extension);
                            if (Data.PhotoFormats.Contains(extension))
                            {
                                EventImage image = new EventImage
                                {
                                    StreamId = new Guid(formFile.Name.Split(".")[0]),
                                    EventId = _event.Id,
                                    Name = sessionImages.Where(x => x.Key == formFile.Name).Select(x => x.Value).FirstOrDefault(),
                                    Size = formFile.Length,
                                    Type = extension,
                                    Image = System.IO.File.ReadAllBytes(formFile.FullName)
                                };

                                _dataUnitOfWork.BaseUow.EventImagesRepository.Add(image);
                                _dataUnitOfWork.BaseUow.EventImagesRepository.SaveChanges();
                                addedimages.Add(image.Id);
                            }
                            else if (Data.DocumentFormats.Contains(extension))
                            {
                                EventDocument doc = new EventDocument
                                {
                                    StreamId = new Guid(formFile.Name.Split(".")[0]),
                                    Document = System.IO.File.ReadAllBytes(formFile.FullName),
                                    Name = sessionDocuments.Where(x => x.Key == formFile.Name).Select(x => x.Value).FirstOrDefault(),
                                    Size = formFile.Length,
                                    Type = extension,
                                    EventId = _event.Id
                                };

                                _dataUnitOfWork.BaseUow.EventDocumentsRepository.Add(doc);
                                _dataUnitOfWork.BaseUow.EventDocumentsRepository.SaveChanges();
                                addeddocs.Add(doc.Id);
                            }
                        }
                    }
                    var reviews = _dataUnitOfWork.BaseUow.EventUsersRepository.GetEventUsers(LoggedUserIds.UserId, _event.Id);
                    _dataUnitOfWork.BaseUow.EventUsersRepository.SetEventReviews(LoggedUserIds.UserId, _event.Id, false);

                    _imageHelper.DropzoneCleaner(files);
                    var existsThumbs = HttpContext.Session.GetObject<Dictionary<string, string>>(SessionKeys.DropzoneTempThumbnails);
                    if (existsThumbs != null)
                    {
                        _imageHelper.DropzoneCleanerTempThumbs(existsThumbs.Select(x => x.Key).ToList());
                    }
                    context.Commit();
                    foreach (var item in reviews)
                    {
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.EventUsers, item.Id, GetControllerName(), GetActionName(), null);
                    }
                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Edit, Tables.Base.Events, _event.Id, GetControllerName(), GetActionName(), null);
                    foreach (var item in addedimages)
                    {
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.EventImages, item, GetControllerName(), GetActionName(), null);
                    }
                    foreach (var item in addeddocs)
                    {
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Add, Tables.Base.EventDocuments, item, GetControllerName(), GetActionName(), null);
                    }
                    return Json(new { success = true, notification = new Notification(NotificationTypes.Success, _localizer.Saved, string.Format(_localizer.SuccessfullySavedAndRecordName, model.Name)).ConvertToJson() });
                }
                catch (Exception ex)
                {
                    context.Rollback();
                    _imageHelper.DropzoneCleaner(files);
                    var existsThumbs = HttpContext.Session.GetObject<Dictionary<string, string>>(SessionKeys.DropzoneTempThumbnails);
                    if (existsThumbs != null)
                    {
                        _imageHelper.DropzoneCleanerTempThumbs(existsThumbs.Select(x => x.Key).ToList());
                    }
                    ModelState.AddModelError(nameof(_localizer.Error), _localizer.Error);

                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Edit, Tables.Base.Roles, model.Id, GetControllerName(), GetActionName(), ex);
                }
            }

            return PartialView(MagicStrings.ViewNames._Edit, model);
        }

        #endregion

        #region Delete
        public JsonResult Delete(int Id)
        {
            Notification notification = null;
            using (IDbContextTransaction contextTransaction = _dataUnitOfWork.BaseUow.EventsRepository.BeginTransaction())
            {
                try
                {
                    var removedEventPayments = new List<int>();
                    var removedEventRegistrations = _dataUnitOfWork.BaseUow.EventRegistrationsRepository.Remove(Id);
                    foreach (var item in removedEventRegistrations)
                    {
                        var id = _dataUnitOfWork.BaseUow.PaymentsRepository.Remove(item);
                        if (id != null)
                        {
                            removedEventPayments.Add(id.Value);
                        }
                    }
                    if (removedEventPayments.Count() > 0)
                        return Json(new Notification(NotificationTypes.Warning, _localizer.Error, string.Format(_localizer.NotSuccessfullyRemovedEvent, removedEventPayments.Count())).ConvertToJson());

                    var model = _dataUnitOfWork.BaseUow.EventsRepository.GetById(Id);
                    var removedPhotos = _dataUnitOfWork.BaseUow.EventImagesRepository.Remove(Id);
                    var removedDocuments = _dataUnitOfWork.BaseUow.EventDocumentsRepository.Remove(Id);
                    var removedEventItems = _dataUnitOfWork.BaseUow.EventItemsRepository.Remove(Id);
                    var removedSponsors = _dataUnitOfWork.BaseUow.EventSponsorsRepository.Remove(Id);
                    var removedEventItemEventTypes = new List<List<int>>();
                    var removedEventLectures = new List<int>();
                    var removedEventLectureLecturers = new List<List<int>>();
                    var removedEventUsers = _dataUnitOfWork.BaseUow.EventUsersRepository.Remove(Id);


                    foreach (var item in removedEventItems)
                    {
                        removedEventItemEventTypes.Add(_dataUnitOfWork.BaseUow.EventItemEventTypesRepository.Remove(item));
                        var id = _dataUnitOfWork.BaseUow.LecturesRepository.Remove(item);
                        if (id != null)
                        {
                            removedEventLectures.Add(id.Value);
                            removedEventLectureLecturers.Add(_dataUnitOfWork.BaseUow.LectureLecturersRepository.Remove(id.Value));
                        }
                    }
                    _dataUnitOfWork.BaseUow.EventsRepository.Remove(model);

                    _dataUnitOfWork.BaseUow.EventsRepository.SaveChanges();

                    contextTransaction.Commit();
                    foreach (var eventLectureLecturers in removedEventLectureLecturers)
                    {
                        foreach (var lectureLecturerId in eventLectureLecturers)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.LectureLecturers, lectureLecturerId, GetControllerName(), GetActionName(), null);
                        }
                    }
                    foreach (var lectureId in removedEventLectures)
                    {
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.Lectures, lectureId, GetControllerName(), GetActionName(), null);
                    }
                    foreach (var registrationId in removedEventRegistrations)
                    {
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.EventRegistrations, registrationId, GetControllerName(), GetActionName(), null);
                    }
                    foreach (var itemId in removedEventItems)
                    {
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.EventItems, itemId, GetControllerName(), GetActionName(), null);
                    }
                    foreach (var paymentId in removedEventPayments)
                    {
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.Payments, paymentId, GetControllerName(), GetActionName(), null);
                    }
                    foreach (var eventItemEventTypes in removedEventItemEventTypes)
                    {
                        foreach (var eventItemEventTypeId in eventItemEventTypes)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.EventItemEventTypes, eventItemEventTypeId, GetControllerName(), GetActionName(), null);
                        }
                    }
                    foreach (var sponsorId in removedSponsors)
                    {
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.EventSponsors, sponsorId, GetControllerName(), GetActionName(), null);
                    }
                    foreach (var documentId in removedDocuments)
                    {
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.EventDocuments, documentId, GetControllerName(), GetActionName(), null);
                    }
                    foreach (var photoId in removedPhotos)
                    {
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.EventImages, photoId, GetControllerName(), GetActionName(), null);
                    }

                    _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.Events, Id, GetControllerName(), GetActionName(), null);
                    notification = new Notification(NotificationTypes.Success, _localizer.Removed, string.Format(_localizer.SuccessfullyRemovedName, model.Name));
                }
                catch (Exception ex)
                {
                    contextTransaction.Rollback();
                    notification = new Notification(NotificationTypes.Error, Localizer.ErrorFriendly, Localizer.AnErrorOccurredFriendly);
                    _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.Events, Id, GetControllerName(), GetActionName(), ex);
                }
            }
            return Json(notification.ConvertToJson());

        }
        public JsonResult DeleteAll(List<int> list)
        {
            var removedEvents = new List<int>();
            var removedEventRegistrations = new List<List<int>>();
            var removedEventPhotos = new List<List<int>>();
            var removedEventDocuments = new List<List<int>>();
            var removedEventPayments = new List<int>();
            var removedEventItems = new List<List<int>>();
            var removedEventItemEventType = new List<List<int>>();
            var removedEventSponsors = new List<List<int>>();
            var removedEventLectures = new List<int>();
            var removedEventLectureLecturers = new List<List<int>>();
            var removedEventUsers = new List<List<int>>();

            Notification notification = null;
            using (IDbContextTransaction contextTransaction = _dataUnitOfWork.BaseUow.EventsRepository.BeginTransaction())
            {
                try
                {
                    foreach (var item in list)
                    {
                        _dataUnitOfWork.BaseUow.EventsRepository.Remove(_dataUnitOfWork.BaseUow.EventsRepository.GetById(item));
                        removedEvents.Add(item);
                        removedEventRegistrations.Add(_dataUnitOfWork.BaseUow.EventRegistrationsRepository.Remove(item));
                        removedEventPhotos.Add(_dataUnitOfWork.BaseUow.EventImagesRepository.Remove(item));
                        removedEventDocuments.Add(_dataUnitOfWork.BaseUow.EventDocumentsRepository.Remove(item));
                        removedEventItems.Add(_dataUnitOfWork.BaseUow.EventItemsRepository.Remove(item));
                        removedEventSponsors.Add(_dataUnitOfWork.BaseUow.EventSponsorsRepository.Remove(item));
                        removedEventUsers.Add(_dataUnitOfWork.BaseUow.EventUsersRepository.Remove(item));
                    }
                    foreach (var eventRegistrations in removedEventRegistrations)
                    {
                        foreach (var registrationId in eventRegistrations)
                        {
                            var id = _dataUnitOfWork.BaseUow.PaymentsRepository.Remove(registrationId);
                            if (id != null)
                            {
                                removedEventPayments.Add(id.Value);
                            }
                        }
                        if (removedEventPayments.Count() > 0)
                            return Json(new Notification(NotificationTypes.Warning, _localizer.Error, string.Format(_localizer.NotSuccessfullyRemovedEvent, removedEventPayments.Count())).ConvertToJson());
                    }
                    foreach (var eventItems in removedEventItems)
                    {
                        foreach (var itemId in eventItems)
                        {
                            removedEventItemEventType.Add(_dataUnitOfWork.BaseUow.EventItemEventTypesRepository.Remove(itemId));
                            var id = _dataUnitOfWork.BaseUow.LecturesRepository.Remove(itemId);
                            if (id != null)
                            {
                                removedEventLectures.Add(id.Value);
                                removedEventLectureLecturers.Add(_dataUnitOfWork.BaseUow.LectureLecturersRepository.Remove(id.Value));
                            }
                        }
                    }

                    _dataUnitOfWork.BaseUow.EventsRepository.SaveChanges();

                    contextTransaction.Commit();
                    foreach (var eventLecutreLecturers in removedEventLectureLecturers)
                    {
                        foreach (var lectureLecturerId in eventLecutreLecturers)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.LectureLecturers, lectureLecturerId, GetControllerName(), GetActionName(), null);
                        }
                    }
                    foreach (var eventUsers in removedEventUsers)
                    {
                        foreach (var eventUserId in eventUsers)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.EventUsers, eventUserId, GetControllerName(), GetActionName(), null);
                        }
                    }
                    foreach (var lectureId in removedEventLectures)
                    {
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.Lectures, lectureId, GetControllerName(), GetActionName(), null);
                    }
                    foreach (var eventItemEventTypes in removedEventItemEventType)
                    {
                        foreach (var eventItemEventTypeId in eventItemEventTypes)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.EventItemEventTypes, eventItemEventTypeId, GetControllerName(), GetActionName(), null);
                        }
                    }
                    foreach (var eventItems in removedEventItems)
                    {
                        foreach (var itemId in eventItems)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.EventItems, itemId, GetControllerName(), GetActionName(), null);
                        }
                    }
                    foreach (var eventSponsors in removedEventSponsors)
                    {
                        foreach (var sponsorId in eventSponsors)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.EventSponsors, sponsorId, GetControllerName(), GetActionName(), null);
                        }
                    }
                    foreach (var eventId in removedEvents)
                    {
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.Events, eventId, GetControllerName(), GetActionName(), null);
                    }
                    foreach (var payementId in removedEventPayments)
                    {
                        _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.Payments, payementId, GetControllerName(), GetActionName(), null);
                    }
                    foreach (var eventRegistrations in removedEventRegistrations)
                    {
                        foreach (var registrationId in eventRegistrations)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.EventRegistrations, registrationId, GetControllerName(), GetActionName(), null);
                        }
                    }
                    foreach (var eventPhotos in removedEventPhotos)
                    {
                        foreach (var photoId in eventPhotos)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.EventImages, photoId, GetControllerName(), GetActionName(), null);
                        }
                    }
                    foreach (var eventDocuments in removedEventDocuments)
                    {
                        foreach (var documentId in eventDocuments)
                        {
                            _logger.Log(Enumerations.LogTypes.Info, Enumerations.LogActivity.Delete, Tables.Base.EventDocuments, documentId, GetControllerName(), GetActionName(), null);
                        }
                    }
                    notification = new Notification(NotificationTypes.Success, Localizer.Removed, string.Format(Localizer.SuccessfullyRemovedName, Localizer.Events));
                }
                catch (Exception ex)
                {
                    contextTransaction.Rollback();
                    notification = new Notification(NotificationTypes.Error, Localizer.ErrorFriendly, Localizer.AnErrorOccurredFriendly);
                    foreach (var item in list)
                    {
                        _logger.Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Delete, Tables.Base.Events, item, GetControllerName(), GetActionName(), ex);
                    }
                }
            }
            return Json(notification.ConvertToJson());

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
                        images.Add(_imageHelper.UploadDropzoneImage(file), file.FileName);
                        HttpContext.Session.SetObject(SessionKeys.Images, images);
                    }
                    else
                    {
                        var documents = HttpContext.Session.GetObject<Dictionary<string, string>>(SessionKeys.Documents);
                        documents.Add(_imageHelper.UploadDropzoneDocument(file), file.FileName);
                        HttpContext.Session.SetObject<Dictionary<string, string>>(SessionKeys.Documents, documents);
                    }
                }
            }
            return Ok();
        }



        #endregion


        public IActionResult RemoveFile(int eventId, string fileName)
        {

            Notification notification = null;
            var di = new DirectoryInfo(_hostingEnvironment.WebRootPath + Paths.DropzoneTemp);
            var diThumb = new DirectoryInfo(_hostingEnvironment.WebRootPath + Paths.DropzoneTempThumbnails);
            try
            {
                if (Data.DocumentFormats.Contains(System.IO.Path.GetExtension(fileName).ToLower()))
                {
                    var sessionDocuments = HttpContext.Session.GetObject<Dictionary<string, string>>(SessionKeys.Documents);
                    var streamId = fileName.Length > 32 ? new Guid(fileName.Split(".")[0]) : default(Guid);
                    var document = _dataUnitOfWork.BaseUow.EventDocumentsRepository.GetByStreamId(streamId, eventId);
                    if (document != null)
                    {
                        _dataUnitOfWork.BaseUow.EventDocumentsRepository.Remove(document);
                        _dataUnitOfWork.BaseUow.EventDocumentsRepository.SaveChanges();
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

                    var image = _dataUnitOfWork.BaseUow.EventImagesRepository.GetByName(fileName, eventId);
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
                                image = _dataUnitOfWork.BaseUow.EventImagesRepository.GetByName(existsThumbs.Where(x => x.Key == fileName).Select(x => x.Value).FirstOrDefault(), eventId);
                            }
                            if (image != null)
                            {
                                _dataUnitOfWork.BaseUow.EventImagesRepository.Remove(image);
                                _dataUnitOfWork.BaseUow.EventImagesRepository.SaveChanges();
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
    }
}