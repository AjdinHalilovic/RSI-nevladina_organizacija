using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NevladinaOrganizacija.Models.Context;

namespace NevladinaOrganizacija.Areas.Administration.Controllers
{
    public class CitizenshipController : Controller
    {
        [Area("Administration")]
        private myContext _dbContext;

        public CitizenshipController(myContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var citizenships = _dbContext.Citizenships.AsEnumerable();
            return View(citizenships);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var citizenship = _dbContext.Citizenships.Find(id);
            return View(citizenship);
        }

        [HttpPost,ValidateAntiForgeryToken]
        public IActionResult Edit(Citizenship citizenship)
        {
            if (!ModelState.IsValid)
            {
                return View(citizenship);
            }
            _dbContext.Citizenships.Update(citizenship);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Add()
        {
            var ctzh = new Citizenship();
            return View(ctzh);
        }

        [HttpPost]
        public IActionResult Add(Citizenship citizenship)
        {
            if (!ModelState.IsValid)
            {
                return View(citizenship);
            }
            _dbContext.Citizenships.Add(citizenship);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var ctzhs = _dbContext.Citizenships.Find(id);
            _dbContext.Citizenships.Remove(ctzhs);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}