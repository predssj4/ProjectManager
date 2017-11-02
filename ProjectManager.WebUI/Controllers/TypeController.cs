using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectManager.Entities;
using ProjectManager.DAL;

namespace ProjectManager.WebUI.Controllers
{
    public class TypeController : Controller
    {
        private ProjectManagerContext db = new ProjectManagerContext();

        [Authorize]
        [Authorize(Roles="admin")]
        public ActionResult Index()
        {
            return View(db.BugTypes.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Create([Bind(Include="ID,Name")] BugType bugtype)
        {
            if (ModelState.IsValid)
            {
                db.BugTypes.Add(bugtype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bugtype);
        }

        [Authorize]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BugType bugtype = db.BugTypes.Find(id);
            if (bugtype == null)
            {
                return HttpNotFound();
            }
            return View(bugtype);
        }

        [Authorize]
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Edit([Bind(Include="ID,Name")] BugType bugtype)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bugtype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bugtype);
        }

        [Authorize]
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BugType bugtype = db.BugTypes.Find(id);
            if (bugtype == null)
            {
                return HttpNotFound();
            }
            return View(bugtype);
        }

        [Authorize]
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            BugType bugtype = db.BugTypes.Find(id);
            db.BugTypes.Remove(bugtype);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
