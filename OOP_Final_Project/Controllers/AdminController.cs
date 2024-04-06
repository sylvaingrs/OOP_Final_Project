using OOP_Final_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace OOP_Final_Project.Controllers
{
    public class AdminController : Controller
    {
        private DBModels db = new DBModels();
        // GET: Admin
        public ActionResult Index()
        {
            return View(db.Account.ToList());
        }

        public ActionResult ExamPage()
        {
            return View(db.Exam);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Account.Find(id);

            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        public ActionResult Create()
        {
            var cohorts = db.Cohort.ToList();
            ViewBag.CohortList = new SelectList(cohorts, "Id", "CohortName");
            ViewBag.Account = new SelectList(db.Exam, "Id", "AccountName");
            return View();
        }

        // POST: Teacher/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FisrtName,LastName,Email,Password,AccountType,CohortId")] Account account, int? cohortId)
        {
            if (ModelState.IsValid)
            {
                account.CohortId = cohortId;

                db.Account.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CohortId = new SelectList(db.Cohort, "CohortId", "CohortId", account.CohortId);
            return View(account);
        }
    }
}