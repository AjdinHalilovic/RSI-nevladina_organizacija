using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nevladinaOrg.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using nevladinaOrg.Web.Constants;

namespace nevladinaOrg.Web.Areas.Administration.Controllers
{
    [Area(MagicStrings.AreaNames.Administration)]
    [WebRoles(Enumerations.WebRoles.SuperAdministrator)]
    public class InstitutionalSettingsController : Controller
    {
        public IActionResult General()
        {
            return View();
        }

        /*dodat General i sa strane samo da ispise sve stavke koje ima u ovom meniju kao i u system settings*/
    }
}