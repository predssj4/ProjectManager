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

namespace ProjectManager.WebUI.Controllers
{
    public class MessageController : Controller
    {
        private ProjectManagerContext db = new ProjectManagerContext();

        public ActionResult Index()
        {
            string currentUserEmail = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            return View(db.Messages.ToList().Where(m => m.Receiver.Email == currentUserEmail));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        public ActionResult Create()
        {
            ViewBag.Users = db.Users.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include="ID,Title,Receiver,Content")] Message message)
        {
            ModelState.Remove("Sender");
            ModelState.Remove("Receiver.Email");
            ModelState.Remove("Receiver.Password");
            if (ModelState.IsValid)
            {
                message.SentDate = DateTime.Now;
                string currentUserEmail = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                message.Sender = db.Users.First(u => u.Email == currentUserEmail);
                message.Receiver = db.Users.First(r => r.ID == message.Receiver.ID);
                db.Messages.Add(message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Users = db.Users.ToList();
            return View(message);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }


        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Message message = db.Messages.Find(id);
            db.Messages.Remove(message);
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
