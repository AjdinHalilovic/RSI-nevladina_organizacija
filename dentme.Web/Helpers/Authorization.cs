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

    public class AsyncActionFilter : IAsyncActionFilter
    {
        private Enumerations.Functionalities[] Functionalities { get; }

        public AsyncActionFilter(Enumerations.Functionalities[] functionalities)
        {
            Functionalities = functionalities;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var loggedUser = context.HttpContext.GetLoggedUserData();

            if (loggedUser?.Functionalities == null)
            {
                context.Result = new RedirectToActionResult("Login", "Access", new { area = string.Empty, redirectTo = context.HttpContext.Request.Path });
                return;
            }
            else if (!loggedUser.PersonUser.ChangePassword)
            {
                //context.Result = new RedirectToActionResult("Users", "ChangePassword", new { area = string.Empty, redirectTo = context.HttpContext.Request.Path });
                //return;
            }

            if (Functionalities.Length == 0)
                await next();

            string controller;
            if (context.Controller.GetType().GetCustomAttributes(typeof(ParentControllerAttribute), true).FirstOrDefault() is ParentControllerAttribute parentControllerAttribute)
                controller = parentControllerAttribute.ControllerName;
            else
                controller = context.RouteData.Values["controller"]?.ToString();

            if (loggedUser.Functionalities.Any(i => string.Equals(i.ControllerName, controller, StringComparison.OrdinalIgnoreCase)))
            {
                if (Functionalities.Any(functionality => loggedUser.Functionalities.Any(i => i.FunctionNumber == (int)functionality)))
                {
                    await next();
                    return;
                }
            }

            context.Result = new RedirectToActionResult("Index", "Error", new { id = 401 });
        }
    }
}