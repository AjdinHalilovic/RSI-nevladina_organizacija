using nevladinaOrg.Web.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace nevladinaOrg.Web.Areas.Administration.Controllers
{
    [Area(MagicStrings.AreaNames.Administration)]
    [Authorization]
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