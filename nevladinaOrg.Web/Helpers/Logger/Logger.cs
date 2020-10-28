using Core.Entities.Base;
using DAL.Repositories.Base.IRepository;
using nevladinaOrg.Web.Constants;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace nevladinaOrg.Web.Helpers.Logger
{
    public class Logger : ILogger
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IActivityLogsRepository _activityLogsRepository;

        public Logger(IHttpContextAccessor httpContextAccessor, IActivityLogsRepository activityLogsRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _activityLogsRepository = activityLogsRepository;
        }

        public ActivityLog Log(Enumerations.LogTypes logType, Enumerations.LogActivity logActivity, string tableName, int rowId, string controller, string action, Exception exception)
        {
            try
            {
                var loggedUserIds = _httpContextAccessor.HttpContext.GetLoggedUserIds().Result;
                var request = _httpContextAccessor.HttpContext.Request;

                var activity = new ActivityLog
                {
                    UserId = loggedUserIds?.UserId ?? 0,
                    InstitutionId = loggedUserIds?.InstitutionId ?? 0,
                    OrganizationId = loggedUserIds?.OrganizationId ?? 0,
                    ActivityId = (int)logActivity,
                    TableName = tableName,
                    RowId = rowId,
                    OccurredAt = DateTime.Now,
                    Controller = controller,
                    ActionMethod = action,
                    ActiveUrl = request.Path.ToUriComponent() ?? string.Empty,
                    ExceptionMessage = exception != null ? (exception.InnerException != null ? $" :: Inner ->  {exception.InnerException.InnerException?.Message}" : exception.Message) : string.Empty,
                    StackTrace = exception != null ? exception.StackTrace : string.Empty,
                    ExceptionType = logType.ToString(),
                    IpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ReferrerUrl = request.Headers["Referer"],
                    WebBrowser = request.Headers["User-Agent"]
                };

                _activityLogsRepository.Add(activity);
                _activityLogsRepository.SaveChanges();

                return activity;
            }
            catch 
            {
                return null;
            }
        }

        public async Task<ActivityLog> LogAsync(Enumerations.LogTypes logType, Enumerations.LogActivity logActivity, string tableName, int rowId, string controller, string action, Exception exception)
        {
            try
            {
                var loggedUserIds = await _httpContextAccessor.HttpContext.GetLoggedUserIds();
                var request = _httpContextAccessor.HttpContext.Request;

                var activity = new ActivityLog
                {
                    UserId = loggedUserIds?.UserId ?? 0,
                    InstitutionId = loggedUserIds?.InstitutionId ?? 0,
                    OrganizationId = loggedUserIds?.OrganizationId ?? 0,
                    ActivityId = (int)logActivity,
                    TableName = tableName,
                    RowId = rowId,
                    OccurredAt = DateTime.Now,
                    Controller = controller,
                    ActionMethod = action,
                    ActiveUrl = request.Path.ToUriComponent() ?? string.Empty,
                    ExceptionMessage = exception != null ? (exception.InnerException != null ? $" :: Inner ->  {exception.InnerException.InnerException?.Message}" : exception.Message) : string.Empty,
                    StackTrace = exception != null ? exception.StackTrace : string.Empty,
                    ExceptionType = logType.ToString(),
                    IpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ReferrerUrl = request.Headers["Referer"],
                    WebBrowser = request.Headers["User-Agent"]
                };

                _activityLogsRepository.Add(activity);
                _activityLogsRepository.SaveChanges();

                return activity;
            }
            catch (Exception ex)
            {
                Log(Enumerations.LogTypes.Error, Enumerations.LogActivity.Undefined, tableName, rowId, controller, action, ex);
            }

            return null;
        }
    }
}