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
using ProjectManager.Domain.Tools;
using System.Web.Security;
using System.Net.Mail;

namespace ProjectManager.WebUI.Controllers
{
    public class BugController : Controller
    {
        private ProjectManagerContext db = new ProjectManagerContext();

        [Authorize]
        public ActionResult Index()
        {
            return View(db.Bugs.ToList());
        }

        [Authorize]
        public ActionResult UserBugs()
        {
            string currentUserEmail = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            return View(db.Users.First(u => u.Email == currentUserEmail).Bugs);
        }

        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bug bug = db.Bugs.Find(id);
            if (bug == null)
            {
                return HttpNotFound();
            }
            return View(bug);
        }

        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Types = db.BugTypes.ToList();
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create([Bind(Include="ID,Name,Type,Description")] Bug bug)
        {
            if (ModelState.IsValid)
            {
                bug.Status = db.BugStatuses.First(b => b.Name == "New");
                bug.Type = db.BugTypes.Find(bug.Type.ID);

                db.Bugs.Add(bug);
                db.SaveChanges();

                sendNotyfication(bug);

                return RedirectToAction("Index");
            }

            ViewBag.Priorities = db.TaskPriorities.ToList();
            return View(bug);
        }

        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Bug bug = db.Bugs.Find(id);

            if (bug == null)
            {
                return HttpNotFound();
            }

            ViewBag.Types = db.BugTypes.ToList();
            return View(bug);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit([Bind(Include="ID,Name,Description")] Bug bug)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bug).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Priorities = db.TaskPriorities.ToList();
            return View(bug);
        }

        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bug bug = db.Bugs.Find(id);
            if (bug == null)
            {
                return HttpNotFound();
            }
            return View(bug);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Bug bug = db.Bugs.Find(id);
            db.Bugs.Remove(bug);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Assign(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Bug bug = db.Bugs.Find(id);
           
            if (bug == null)
            {
                return HttpNotFound();
            }

            string currentUserEmail = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            bug.User = db.Users.First(u => u.Email == currentUserEmail);

            bug.Status = db.BugStatuses.First(b => b.Name == "Assigned");

            db.Entry(bug).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Resolve(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Bug bug = db.Bugs.Find(id);

            if (bug == null)
            {
                return HttpNotFound();
            }

            bug.Status = db.BugStatuses.First(b => b.Name == "Resolved");

            db.Entry(bug).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("UserBugs");
        }

        [Authorize]
        public ActionResult Reopen(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Bug bug = db.Bugs.Find(id);

            if (bug == null)
            {
                return HttpNotFound();
            }

            bug.Status = db.BugStatuses.First(b => b.Name == "Reopen");
            // Remove relationship with user
            db.Entry(bug).Reference(r => r.User).CurrentValue = null;
            
            db.Entry(bug).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("UserBugs");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void sendNotyfication(Bug bug)
        {
            User admin = db.Users.Find(1);

            MailMessage message = new MailMessage(new MailAddress(admin.Email), new MailAddress(admin.Email));
            message.Subject = "Reported new bug";
            message.IsBodyHtml = true;
            message.Body = "<p>Name: " + bug.Name + "</p><p>Type: " + bug.Type.Name + "</p><p>Description: " + bug.Description + "</p>";

            GmailClient gmailClient = new GmailClient();
            gmailClient.sendMessage(message);
        }
    }
}
