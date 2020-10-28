using nevladinaOrg.Web.Helpers;
using nevladinaOrg.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DAL;
using Core.Entities.Base.DTO;
using System.Collections.Generic;
using System.Linq;

namespace nevladinaOrg.Web.Controllers
{
    [Authorization]
    public class HomeController : Controller
    {
        private IDataUnitOfWork _dataUnitOfWork;
        public HomeController(IDataUnitOfWork dataUnitOfWork)
        {
            _dataUnitOfWork = dataUnitOfWork;
        }
        public IActionResult Index()
        {
            TempData[MagicStrings.TempDataNames.jsNotifications] = new Notification(NotificationTypes.Error, "Dobro dosli", "Beta verzija").ConvertToJson();
            DashboardViewModel model = new DashboardViewModel
            {
                Events = _dataUnitOfWork.BaseUow.EventsDTORepository.GetAll().Take(5)
            };
            return View(model);
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
