using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NevladinaOrganizacija.Models.Context;

namespace NevladinaOrganizacija.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class CitiesController : Controller
    {
        #region Properties
        private myContext _myContext;
        #endregion

        public CitiesController(myContext myContext)
        {
            _myContext = myContext;
        }

        public IActionResult Index()
        {
            var model = _myContext.Cities;
            return View(model);
        }
        public IActionResult Add()
        {
            return View(new City());
        }
        [HttpPost]
        public IActionResult Add(City model)
        {
            if (ModelState.IsValid)
            {
                _myContext.Add(model);
                _myContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int Id)
        {
            var model = _myContext.Cities.FirstOrDefault(x => x.Id == Id);
            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(City model)
        {
            if (ModelState.IsValid)
            {
                _myContext.Update(model);
                _myContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Delete(int Id)//5
        {
            _myContext.Remove(_myContext.Cities.FirstOrDefault(x => x.Id == Id));
            _myContext.SaveChanges();
            return RedirectToAction("Index");
        }

 
    }
}