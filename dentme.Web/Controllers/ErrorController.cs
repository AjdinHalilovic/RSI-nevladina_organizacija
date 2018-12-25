using Microsoft.AspNetCore.Mvc;

using nevladinaOrg.Web.Constants;

namespace nevladinaOrg.Web.Controllers
{
    public class ErrorController : Controller
    {
        [Route(CustomRoutes.Error)]
        public IActionResult Index(int? id)
        {
            switch (id)
            {
                case 300:
                    return View("BadRequest");
                case 401:
                    return View("Unauthorized");
                case 404:
                    return View("NotFound");
                default:
                    return View("InternalServerError");
            }
        }
    }
}