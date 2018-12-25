using Core.Entities.Base;
using DAL;
using DAL.Repositories.Base.IRepository;
using nevladinaOrg.Web.Constants;
using nevladinaOrg.Web.ViewModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace nevladinaOrg.Web.Helpers
{
    public static class Extensions
    {
        #region String

        public static string Format(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        public static bool IsSet(this string source)
        {
            return !string.IsNullOrWhiteSpace(source);
        }

        public static string Remove(this string source, string value)
        {
            return source.Replace(value, string.Empty);
        }

        public static string RemoveTags(this string source)
        {
            return Regex.Replace(source, @"<[^>]*>", string.Empty);
        }

        public static string RemoveControllerNameSuffix(this string source) => source.Remove("Controller");

        #endregion

        #region Session

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static void SetObject<T>(this ISession session, string key, T value) => session.SetString(key, JsonConvert.SerializeObject(value));

        public static void RemoveObject(this ISession session, string key) => session.Remove(key);

        #endregion

        #region Cookie

        public static T GetObject<T>(this HttpRequest request, string key) => request.Cookies[key] == null ? default(T) : JsonConvert.DeserializeObject<T>(request.Cookies[key]);

        public static void SetObject(this HttpResponse response, string key, object value, int? expireTime = null)
        {
            if (value == null)
            {
                response.Cookies.Delete(key);
                return;
            }

            var json = JsonConvert.SerializeObject(value);
            var options = new CookieOptions
            {
                Expires = expireTime.HasValue ? DateTime.Now.AddMinutes(expireTime.GetValueOrDefault()) : DateTime.Now.AddDays(7)
            };

            response.Cookies.Append(key, json, options);
        }

        public static void RemoveObject(this HttpResponse response, string key) => response.Cookies.Delete(key);

        #endregion

        #region HttpContext

        public static async Task<LoggedUserIdsViewModel> GetLoggedUserIds(this HttpContext httpContext)
        {
            var loggedUserIds = httpContext.Session.GetObject<LoggedUserIdsViewModel>(SessionKeys.LoggedUserIds);
            if (loggedUserIds != null)
            {
                return loggedUserIds;
            }

            var token = httpContext.Request.GetObject<string>(SessionKeys.LoggedUserData);
            if (string.IsNullOrEmpty(token))
                return null;

            bool institutionUser = token.StartsWith("IU");
            Guid userToken = Guid.Empty;
            Guid.TryParse(token.Substring(2), out userToken);

            var dataUnitOfWork = (IDataUnitOfWork)httpContext.RequestServices.GetService(typeof(IDataUnitOfWork));

            var user = await dataUnitOfWork.BaseUow.UsersDTORepository.GetByTokenAsync(userToken, institutionUser);
            if (user == null)
                return null;

            IEnumerable<Functionality> functionalities = null;
            if (user.OrganizationInstitutionUserId.HasValue)
                functionalities = await dataUnitOfWork.BaseUow.FunctionalitiesRepository.GetByOrganizationInstitutionUserIdAsync(user.OrganizationInstitutionUserId.Value);
            else
                functionalities = await dataUnitOfWork.BaseUow.FunctionalitiesRepository.GetByInstitutionUserIdAsync(user.InstitutionUserId);

            /* UKLJUCITI OVO KASNIJE !!! */

            //if (functionalities == null)
            //    return null;

            var loggedUserData = new LoggedUserDataViewModel
            {
                User = user,
                PersonUser = dataUnitOfWork.BaseUow.PersonUsersDTORepository.GetById(user.UserId),
                Functionalities = functionalities
            };

            loggedUserIds = new LoggedUserIdsViewModel
            {
                UserId = user.Id,
                InstitutionId = user.InstitutionId,
                InstitutionUserId = user.InstitutionUserId,
                MemberId = user.MemberId,
                OrganizationId = user.OrganizationId,
                OrganizationInstitutionUserId = user.OrganizationInstitutionUserId
            };

            httpContext.Session.SetObject(SessionKeys.LoggedUserIds, loggedUserIds);
            httpContext.Session.SetObject(SessionKeys.LoggedUserData, loggedUserData);

            return loggedUserIds;
        }

        public static LoggedUserDataViewModel GetLoggedUserData(this HttpContext httpContext)
        {
            var loggedUserData = httpContext.Session.GetObject<LoggedUserDataViewModel>(SessionKeys.LoggedUserData);
            if (loggedUserData != null)
                return loggedUserData;

            var token = httpContext.Request.GetObject<string>(SessionKeys.LoggedUserData);
            if (string.IsNullOrEmpty(token))
                return null;

            bool institutionUser = token.StartsWith("IU");
            Guid userToken = Guid.Empty;
            Guid.TryParse(token.Substring(2), out userToken);

            var dataUnitOfWork = (IDataUnitOfWork)httpContext.RequestServices.GetService(typeof(IDataUnitOfWork));

            var user = dataUnitOfWork.BaseUow.UsersDTORepository.GetByToken(userToken, institutionUser);
            if (user == null)
                return null;

            IEnumerable<Functionality> functionalities = null;
            if (user.OrganizationInstitutionUserId.HasValue)
                functionalities = dataUnitOfWork.BaseUow.FunctionalitiesRepository.GetByOrganizationInstitutionUserId(user.OrganizationInstitutionUserId.Value);
            else
                functionalities = dataUnitOfWork.BaseUow.FunctionalitiesRepository.GetByInstitutionUserId(user.InstitutionUserId);

            /* UKLJUCITI OVO KASNIJE !!! */

            //if (functionalities == null)
            //    return null;

            loggedUserData = new LoggedUserDataViewModel
            {
                User = user,
                PersonUser = dataUnitOfWork.BaseUow.PersonUsersDTORepository.GetById(user.UserId),
                Functionalities = functionalities
            };

            var loggedUserIds = new LoggedUserIdsViewModel
            {
                UserId = user.Id,
                InstitutionId = user.InstitutionId,
                InstitutionUserId = user.InstitutionUserId,
                MemberId = user.MemberId,
                OrganizationId = user.OrganizationId,
                OrganizationInstitutionUserId = user.OrganizationInstitutionUserId
            };

            httpContext.Session.SetObject(SessionKeys.LoggedUserIds, loggedUserIds);
            httpContext.Session.SetObject(SessionKeys.LoggedUserData, loggedUserData);

            return loggedUserData;
        }

        public static IEnumerable<Enumerations.Functionalities> GetUserFunctionalities(this HttpContext httpContext, string controllerName)
        {
            controllerName = controllerName.RemoveControllerNameSuffix();

            return httpContext.GetLoggedUserData().Functionalities.Where(f => f.ControllerName.Equals(controllerName, StringComparison.OrdinalIgnoreCase)).Select(f => (Enumerations.Functionalities)f.FunctionNumber).ToArray();
        }

        public static void SetLoggedUser(this HttpContext httpContext, LoggedUserDataViewModel loggedUserDataViewModel, bool rememberMe = false)
        {
            if (loggedUserDataViewModel == null)
            {
                httpContext.Session.RemoveObject(SessionKeys.LoggedUserIds);
                httpContext.Session.RemoveObject(SessionKeys.LoggedUserData);

                httpContext.Response.RemoveObject(SessionKeys.LoggedUserData);
                return;
            }

            httpContext.Session.SetObject(SessionKeys.LoggedUserData, loggedUserDataViewModel);
            httpContext.Session.SetObject(SessionKeys.LoggedUserIds, new LoggedUserIdsViewModel
            {
                UserId = loggedUserDataViewModel.User.UserId,
                InstitutionId = loggedUserDataViewModel.User.InstitutionId,
                InstitutionUserId = loggedUserDataViewModel.User.InstitutionUserId,
                MemberId = loggedUserDataViewModel.User.MemberId,
                OrganizationId = loggedUserDataViewModel.User.OrganizationId,
                OrganizationInstitutionUserId = loggedUserDataViewModel.User.OrganizationInstitutionUserId
            });

            string token = loggedUserDataViewModel.User.OrganizationInstitutionUserId.HasValue ? $"OU{loggedUserDataViewModel.User.Token}" : $"IU{loggedUserDataViewModel.User.Token}";
            httpContext.Response.SetObject(SessionKeys.LoggedUserData, rememberMe ? token : string.Empty);
        }

        public static bool IsUserLogged(this HttpContext httpContext)
        {
            return httpContext.GetLoggedUserIds() != null;
        }

        #endregion

        #region ViewData

        public static object Title(this ViewDataDictionary viewDataDictionary)
        {
            return viewDataDictionary["Title"];
        }

        public static void Title(this ViewDataDictionary viewDataDictionary, string title)
        {
            viewDataDictionary["Title"] = title;
        }

        public static object ActiveItem(this ViewDataDictionary viewDataDictionary)
        {
            return viewDataDictionary["ActiveItem"];
        }

        public static void ActiveItem(this ViewDataDictionary viewDataDictionary, SideBarNavigationItems activeItem)
        {
            viewDataDictionary["ActiveItem"] = activeItem;
        }

        public static object ActiveSubItem(this ViewDataDictionary viewDataDictionary)
        {
            return viewDataDictionary["ActiveSubItem"];
        }

        public static void ActiveSubItem(this ViewDataDictionary viewDataDictionary, SideBarNavigationItems activeSubItem)
        {
            viewDataDictionary["ActiveSubItem"] = activeSubItem;
        }

        #endregion

        #region Object

        public static bool HasProperty(this object source, string propertyName)
        {
            return source.GetType().GetProperty(propertyName) != null;
        }

        #endregion

        #region IHtmlContent

        public static string GetAttribute(this IHtmlContent content, string attribute)
        {
            var writer = new StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);

            return new Regex($@"(?<=\b{attribute}="")[^""]*").Match(writer.ToString()).Value;
        }

        #endregion

        #region IFormFile

        public static bool IsPhoto(this IFormFile source)
        {
            return source.Length > 0 && source.ContentType.Contains("image");
        }

        #endregion

        #region Exception

        public static string InnerExceptionMessage(this Exception source)
        {
            return source == null ? string.Empty : source.InnerException == null ? source.Message : source.InnerException.Message;
        }

        #endregion

        #region byte[]

        public static string ToBase64(this byte[] source)
        {
            return source == null ? string.Empty : Convert.ToBase64String(source);
        }

        public static string ToImageSrc(this byte[] source)
        {
            return source == null ? string.Empty : $"data:image/jpeg;base64,{source.ToBase64()}";
        }

        public static byte[] ToByteArray(this IFormFile source)
        {
            if (source == null)
                return null;

            using (var memoryStream = new MemoryStream())
            {
                source.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        #endregion
    }
}