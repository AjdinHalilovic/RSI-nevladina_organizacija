using System;
using System.Linq;

using Microsoft.AspNetCore.Http;

using nevladinaOrg.Web.Constants;

namespace nevladinaOrg.Web.Helpers.UserFunctionalities
{
    public class UserFunctionalities : IUserFunctionalities
    {
        #region Functionalities

        public bool Index { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public bool Details { get; set; }
        public bool ChangeStatus { get; set; }

        public bool SystemSettings { get; set; }
        public bool GeneralSettings { get; set; }
        public bool RegionalSettings { get; set; }

        public bool AnyAction => Edit || Delete || Details || ChangeStatus;
        public bool AnySettings => SystemSettings || GeneralSettings || RegionalSettings;

        #endregion

        #region Fields and constructor

        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserFunctionalities(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Prepare(string controllerName)
        {
            var functionalities = _httpContextAccessor.HttpContext.GetUserFunctionalities(controllerName).ToList();
            functionalities.ForEach(item =>
            {
                if(!this.HasProperty(item.ToString()))
                    throw new Exception($"Nedostaje funkcionalnost {item}.");

                GetType().GetProperty(item.ToString()).SetValue(this, true);
            });
        }

        public bool IsAuthorized(string controllerName, Enumerations.Functionalities functionality)
        {
            return _httpContextAccessor.HttpContext.GetUserFunctionalities(controllerName).Contains(functionality);
        }

        #endregion
    }
}