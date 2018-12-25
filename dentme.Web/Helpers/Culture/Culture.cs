using System;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

using nevladinaOrg.Web.Constants;

namespace nevladinaOrg.Web.Helpers.Culture
{
    public class Culture : ICulture
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Culture(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Get()
        {
            return _httpContextAccessor.HttpContext.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
        }

        public void Set(string value)
        {
            Delete();
            _httpContextAccessor.HttpContext.Response.Cookies
                .Append(CookieRequestCultureProvider.DefaultCookieName, 
                        CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(value ?? Configuration.DefaultCulture)), 
                        new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
        }

        public void Delete()
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(CookieRequestCultureProvider.DefaultCookieName);
        }
    }
}