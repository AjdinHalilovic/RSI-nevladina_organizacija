using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using nevladinaOrg.Web.Constants;
using nevladinaOrg.Web.Helpers.Attributes;

namespace nevladinaOrg.Web.Helpers
{
    public class Authorization : TypeFilterAttribute
    {
        public Authorization(params Enumerations.Functionalities[] functionalities) : base(typeof(AsyncActionFilter))
        {
            Arguments = new object[]
            {
                functionalities
            };
        }
    }

    public class WebRoles : TypeFilterAttribute
    {
        public WebRoles(params Enumerations.WebRoles[] roles) : base(typeof(AsyncActionFilter))
        {
            Arguments = new object[]
            {
                roles
            };
        }
    }

    public class AsyncActionFilter : IAsyncActionFilter
    {
        private Enumerations.WebRoles[] Roles { get; }

        public AsyncActionFilter(Enumerations.WebRoles[] roles)
        {
            Roles = roles;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var loggedUser = context.HttpContext.GetLoggedUserData();

            if (loggedUser?.Roles == null)
            {
                context.Result = new RedirectToActionResult("Login", "Access", new { area = string.Empty, redirectTo = context.HttpContext.Request.Path });
                return;
            }
            else if (!loggedUser.PersonUser.ChangePassword)
            {
                //context.Result = new RedirectToActionResult("Users", "ChangePassword", new { area = string.Empty, redirectTo = context.HttpContext.Request.Path });
                //return;
            }

            if (Roles.Length == 0)
                await next();

            string controller;
            if (context.Controller.GetType().GetCustomAttributes(typeof(ParentControllerAttribute), true).FirstOrDefault() is ParentControllerAttribute parentControllerAttribute)
                controller = parentControllerAttribute.ControllerName;
            else
                controller = context.RouteData.Values["controller"]?.ToString();

            if (Roles.Any(role => loggedUser.Roles.Any(i => i.Code <= (int)role)))
            {
                await next();
                return;
            }

            context.Result = new RedirectToActionResult("Index", "Error", new { id = 401 });
        }
    }
}