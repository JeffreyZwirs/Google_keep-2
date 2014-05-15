using Google_keep___2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Web.Helpers;
using System.Web.Security;
using WebMatrix.WebData;
using Google_keep___2.Filters;

namespace Google_keep___2.Controllers
{
    [InitializeSimpleMembership] 
    public class HomeController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index(string sortNotitie, string currentFilter, string searchString, int? page)
        {
            ViewBag.NotitieMainSortParm = String.IsNullOrEmpty(sortNotitie) ? "main desc" : "";
            ViewBag.NotitieSortParm = sortNotitie == "notitie" ? "submain desc" : "notitie";

            if (Request.HttpMethod == "GET")
            {
                searchString = currentFilter;
            }
            else
            {
                page = 1;
            }
            ViewBag.CurrentFilter = searchString;

            var UserId = WebSecurity.GetUserId(User.Identity.Name);
            var Notities = from n in db.Notities.Where(n => n.UserProfiles.UserId == UserId) select n;

            if (!String.IsNullOrEmpty(searchString))
            {
                Notities = Notities.Where(n => n.NotitieTitle.ToUpper().Contains(searchString.ToUpper())
                                       || n.NGroups.NotitieGroup.ToUpper().Contains(searchString.ToUpper())
                                       || n.NotitieDescription.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortNotitie)
            {
                case "main desc":
                    Notities = Notities.OrderByDescending(n => n.NotitieTitle);
                    Notities = Notities.OrderByDescending(n => n.NGroups.NotitieGroup);
                    break;
                case "notitie":
                    Notities = Notities.OrderBy(n => n.NotitieTitle);
                    Notities = Notities.OrderBy(n => n.NGroups.NotitieGroup);
                    Notities = Notities.OrderBy(n => n.NotitieDescription);
                    break;
                case "submain desc":
                    Notities = Notities.OrderByDescending(n => n.NotitieTitle);
                    Notities = Notities.OrderByDescending(n => n.NGroups.NotitieGroup);
                    Notities = Notities.OrderByDescending(n => n.NotitieDescription);
                    break;
                default:
                    Notities = Notities.OrderBy(n => n.NotitieTitle);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(Notities.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {
            ViewBag.UserId = WebSecurity.GetUserId(User.Identity.Name);
            ViewBag.UserName = User.Identity.Name;
            ViewBag.NotitieGroupId = new SelectList(db.NotitieGroups, "NotitieGroupId", "NotitieGroup");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Notities Notitie)
        {
            if (ModelState.IsValid)
            {
                db.Notities.Add(Notitie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }           
            ViewBag.NotitieGroupId = new SelectList(db.NotitieGroups, "NotitieGroupId", "NotitieGroup", Notitie.NotitieGroupId);
            return View(Notitie);
        }

        public ActionResult Delete(int id = 0)
        {
            Notities Notitie = db.Notities.Find(id);
            if (Notitie == null)
            {
                return HttpNotFound();
            }
            return View(Notitie);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Notities Notitie = db.Notities.Find(id);
            db.Notities.Remove(Notitie);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id = 0)
        {
            Session["NotitieId"] = id;
            Notities Notitie = db.Notities.Find(id);
            if (Notitie == null)
            {
                return HttpNotFound();
            }
            ViewBag.NotitieGroupId = new SelectList(db.NotitieGroups, "NotitieGroupId", "NotitieGroup", Notitie.NotitieGroupId);
            return View(Notitie);
        }

        [HttpPost]
        public ActionResult Edit(Notities Notitie)
        {
            Notitie.NotitieId = (int)Session["NotitieId"];
            if (ModelState.IsValid)
            {
                db.Entry(Notitie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.NotitieGroupId = new SelectList(db.NotitieGroups, "NotitieGroupId", "NotitieGroup", Notitie.NotitieGroupId);
            return View(Notitie);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
