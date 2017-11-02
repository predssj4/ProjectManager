using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectManager.WebUI.Controllers
{
    public class StatisticsController : Controller
    {

        private ProjectManagerContext db = new ProjectManagerContext();

        public ActionResult Index()
        {
            ViewBag.Tasks = db.Tasks.ToList().Count();
            ViewBag.FinishedTasks = db.Tasks.Where(t => t.Status.Name == "Finished").ToList().Count();

            ViewBag.Bugs = db.Bugs.ToList().Count();
            ViewBag.ResolvedBugs = db.Bugs.Where(b => b.Status.Name == "Resolved").ToList().Count();

            return View();
        }
	}
}