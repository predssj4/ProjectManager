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
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ProjectManager.WebUI.Controllers
{
    public class DocumentController : Controller
    {
        private ProjectManagerContext db = new ProjectManagerContext();

        [Authorize]
        public ActionResult Index()
        {
            return View(db.Documents.ToList());
        }

        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        [Authorize]
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Create([Bind(Include="ID,Name,Description")] Document document)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];

                    if (file != null && file.ContentLength > 0)
                    {
                        MD5 md5 = new MD5CryptoServiceProvider();
                        
                        Byte[] oryginalFileName = ASCIIEncoding.Default.GetBytes(DateTime.Now.ToString());
                        Byte[] hashedFileName = md5.ComputeHash(oryginalFileName);

                        var fileName = BitConverter.ToString(hashedFileName) + Path.GetExtension(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Documents/"), fileName);
                        file.SaveAs(path);

                        document.Path = "Documents/" + fileName;

                        db.Documents.Add(document);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }

            return View(document);
        }

        [Authorize]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        [Authorize]
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Edit([Bind(Include="ID,Name,Description")] Document document)
        {
            if (ModelState.IsValid)
            {
                document.Path = db.Documents.Find(document.ID).Path;

                db.Entry(document).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(document);
        }

        [Authorize]
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        [Authorize]
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Document document = db.Documents.Find(id);

            System.IO.File.Delete(Server.MapPath("~/" + document.Path));

            db.Documents.Remove(document);
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
