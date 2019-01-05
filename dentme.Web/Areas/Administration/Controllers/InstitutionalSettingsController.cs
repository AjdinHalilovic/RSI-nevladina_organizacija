using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nevladinaOrg.Web.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace nevladinaOrg.Web.Areas.Administration.Controllers
{
    public class InstitutionalSettingsController : Controller
    {
        [Area(MagicStrings.AreaNames.Administration)]
        [Authorization]
        public IActionResult General()
        {
            return View();
        }

        /*dodat General i sa strane samo da ispise sve stavke koje ima u ovom meniju kao i u system settings*/
    }
}