using nevladinaOrg.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using nevladinaOrg.Web.Constants;

namespace nevladinaOrg.Web.Areas.Administration.Controllers
{
    [Area(MagicStrings.AreaNames.Administration)]
    [WebRoles(Enumerations.WebRoles.Administrator)]
    public class SystemSettingsController : Controller
    {
        public IActionResult General()
        {
            return View();
        }
        public IActionResult Regional()
        {
            return View();
        }
    }
}