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
using System.Web.Security;
using System.Net.Mail;
using ProjectManager.Domain.Tools;

namespace ProjectManager.WebUI.Controllers
{
    public class TaskController : Controller
    {
        private ProjectManagerContext db = new ProjectManagerContext();

        public ActionResult Index()
        {
            return View(db.Tasks.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        public ActionResult Create()
        {
            ViewBag.Priorities = db.TaskPriorities.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include="ID,Name,Priority,Description,Deadline")] Task task)
        {
            if (ModelState.IsValid)
            {
                task.Priority = db.TaskPriorities.Find(task.Priority.ID);
                task.Status = db.TaskStatuses.First(s => s.Name == "New");

                db.Tasks.Add(task);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Priorities = db.TaskPriorities.ToList();
            return View(task);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Task task = db.Tasks.Find(id);

            if (task == null)
            {
                return HttpNotFound();
            }

            ViewBag.Priorities = db.TaskPriorities.ToList();

            return View(task);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include="ID,Name,Prority,Description,Deadline")] Task task)
        {
            if (ModelState.IsValid)
            {
                if (task.Priority == null)
                {
                    task.Priority = db.TaskPriorities.Find(1);
                }
                else
                {
                    task.Priority = db.TaskPriorities.Find(task.Priority.ID);
                }

                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Priorities = db.TaskPriorities.ToList();
            return View(task);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Task task = db.Tasks.Find(id);
            db.Tasks.Remove(task);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UserTasks()
        {
            string currentUserEmail = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            return View(db.Users.First(u => u.Email == currentUserEmail).Tasks);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Assign(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Task task = db.Tasks.Find(id);

            if (task == null)
            {
                return HttpNotFound();
            }

            string currentUserEmail = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            task.User = db.Users.First(u => u.Email == currentUserEmail);

            task.Status = db.TaskStatuses.First(b => b.Name == "Assigned");

            db.Entry(task).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Finish(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Task task = db.Tasks.Find(id);

            if (task == null)
            {
                return HttpNotFound();
            }

            task.Status = db.TaskStatuses.First(s => s.Name == "Finished");

            db.Entry(task).State = EntityState.Modified;
            db.SaveChanges();

            this.sendNotyfication(task);

            return RedirectToAction("Index");
        }

        public ActionResult Reopen(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Task task = db.Tasks.Find(id);

            if (task == null)
            {
                return HttpNotFound();
            }

            task.Status = db.TaskStatuses.First(s => s.Name == "Reopen");
            db.Entry(task).Reference(t => t.User).CurrentValue = null;

            db.Entry(task).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        private void sendNotyfication(Task task)
        {
            User admin = db.Users.Find(1);

            MailMessage message = new MailMessage(new MailAddress(admin.Email), new MailAddress(admin.Email));
            message.Subject = "Finished task";
            message.IsBodyHtml = true;
            message.Body = "<p>Name: " + task.Name + "</p><p>Priority: " + task.Priority.Name + "</p><p>Description: " + task.Description + "</p>";

            GmailClient gmailClient = new GmailClient();
            gmailClient.sendMessage(message);
        }
    }
}
