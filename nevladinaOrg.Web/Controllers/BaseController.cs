using nevladinaOrg.Web.Helpers;
using nevladinaOrg.Web.Helpers.Breadcrumb;
using nevladinaOrg.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System;

namespace nevladinaOrg.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly Localizer Localizer;
        protected readonly IBreadcrumb Breadcrumb;
        protected LoggedUserIdsViewModel LoggedUserIds => HttpContext?.GetLoggedUserIds().Result;
        protected LoggedUserDataViewModel LoggedUserData => HttpContext?.GetLoggedUserData();
        protected Uri BaseUri => new Uri($"{Request.Scheme}://{Request.Host}{Request.PathBase}");

        protected BaseController(IBreadcrumb breadcrumb, IStringLocalizerFactory stringLocalizerFactory)
        {
            Localizer = new Localizer(stringLocalizerFactory);
            Breadcrumb = breadcrumb;

            Breadcrumb.ClearExistingItems();
            Breadcrumb.AddItem(nameof(HomeController), nameof(HomeController.Index), Localizer.HomePage);
        }

        protected string GetActionName() => HttpContext?.GetRouteData()?.Values["action"]?.ToString();
        protected string GetControllerName() => HttpContext?.GetRouteData()?.Values["controller"]?.ToString();

        /* Return the modal error partial view. */
        protected PartialViewResult ModalErrorPartialView(string message = null, bool badRequest = true)
        {
            if(badRequest)
                SetBadRequest();

            return PartialView("_ModalError", message);
        }

        /* Return the error view. */
        protected ViewResult ErrorView(string message = null) => View("Error", message);

        /* Return the error partial view. */
        protected PartialViewResult ErrorPartialView(string message = null) => PartialView("_Error", message);

        /* Sets bad request and returns the partial view. */
        protected PartialViewResult BadRequestPartialView(string partialViewName, object model)
        {
            SetBadRequest();

            return PartialView(partialViewName, model);
        }

        protected void SetBadRequest()
        {
            Response.StatusCode = 400;
        }
    }
}