using Google_keep___2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Google_keep___2.Controllers
{
    public class NotitieGroupController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        
        public ViewResult Index()
        {
            var Groups = db.NotitieGroups;
            return View(Groups.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(NotitieGroups NotitieGroups)
        {
            if (ModelState.IsValid)
            {
                db.NotitieGroups.Add(NotitieGroups);
                db.SaveChanges();
                return RedirectToAction("Create", "Home");
            }

            return View(NotitieGroups);
        }

        public ActionResult Delete(int id = 0)
        {
            NotitieGroups Groups = db.NotitieGroups.Find(id);
            if (Groups == null)
            {
                return HttpNotFound();
            }
            return View(Groups);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            NotitieGroups Groups = db.NotitieGroups.Find(id);
            db.NotitieGroups.Remove(Groups);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
