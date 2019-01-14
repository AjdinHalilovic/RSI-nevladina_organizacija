using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Base;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace nevladinaOrg.Web.Controllers
{
    public class TestController : Controller
    {
        private IDataUnitOfWork _dataUnitOfWork;

        public TestController(IDataUnitOfWork dataUnitOfWork)
        {
            _dataUnitOfWork = dataUnitOfWork;
        }
        public IActionResult Index()
        {
            try
            {
                var users = _dataUnitOfWork.BaseUow.UsersDTORepository.GetAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
            }

            return View();
        }
    }
}