using Core.Entities.Base;
using nevladinaOrg.Web.Constants;
using System;

namespace nevladinaOrg.Web.Helpers.Logger
{
    public interface ILogger
    {
        ActivityLog Log(Enumerations.LogTypes logType, Enumerations.LogActivity logActivity, string tableName, int rowId, string controller, string action, Exception exception = null);
    }
}