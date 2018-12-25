using nevladinaOrg.Web.Helpers;
using nevladinaOrg.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace nevladinaOrg.Web.Controllers
{
    [Authorization]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            TempData[MagicStrings.TempDataNames.jsNotifications] = new Notification(NotificationTypes.Error, "Dobro dosli", "Beta verzija").ConvertToJson();
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
