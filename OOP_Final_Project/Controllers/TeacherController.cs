using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OOP_Final_Project.Models;

namespace OOP_Final_Project.Controllers
{
    public class TeacherController : Controller
    {
        private DBModels db = new DBModels();

        // GET: Teacher
        public ActionResult Index()
        {
            if (Session["id"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int Id = (int)Session["id"];

            Account account = db.Account.Find(Id);

            if (account == null)
            {
                return HttpNotFound();
            }

            var cohorts = db.Cohort
                .Where(cohort => cohort.Course.Any(course => course.TeacherId == account.Id))
                .ToList();

            return View(cohorts);
        }

        // GET: Teacher/CohortPage
        public ActionResult CohortPage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var cohort = db.Cohort.Find(id);

            var students = db.Account.Where(account => account.CohortId == cohort.Id).ToList();

            ViewBag.Title = cohort.CohortName;
            return View(students);
        }

        public ActionResult ExamPage()
        {
            return View(db.Exam);
        }

        // GET: Teacher/Details/5
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

            ViewBag.CohortId = account.CohortId;
            return View(account);
        }

        public ActionResult DetailsExam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exam exam = db.Exam.Find(id);
            if (exam == null)
            {
                return HttpNotFound();
            }
            return View(exam);
        }

        // GET: Teacher/Create
        public ActionResult Create()
        {
            ViewBag.CohortId = new SelectList(db.Exam, "Id", "ExamName");
            return View();
        }

        // POST: Teacher/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,CourseId,Coefficient")] Exam exam)
        {
            if (ModelState.IsValid)
            {
                db.Exam.Add(exam);
                db.SaveChanges();
                return RedirectToAction("ExamPage");
            }

            ViewBag.CohortId = new SelectList(db.Exam, "Id", "ExamName", exam.Id);
            return View(exam);
        }

        // GET: Teacher/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.CohortId = new SelectList(db.Cohort, "Id", "CohortName", account.CohortId);
            return View(account);
        }

        public ActionResult EditExam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exam exam = db.Exam.Find(id);
            if (exam == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.Exam, "Id", "ExamName", exam.Id);
            ViewBag.CourseId = new SelectList(db.Course, "Id", "CouseId", exam.CourseId);
            return View(exam);
        }

        // POST: Teacher/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FisrtName,LastName,Email,Password,AccountType,CohortId")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CohortId = new SelectList(db.Cohort, "Id", "CohortName", account.CohortId);
            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditExam([Bind(Include = "Id,Date,CourseId,Coefficient")]Exam exam)
        {
            if (ModelState.IsValid)
            {
                db.Entry(exam).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ExamPage");
            }
            ViewBag.Id = new SelectList(db.Cohort, "Id", "ExamName", exam.Id);
            return View(exam);
        }

        // GET: Teacher/Delete/5
        public ActionResult Delete(int? id)
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

        public ActionResult DeleteExam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exam exam = db.Exam.Find(id);
            if (exam == null)
            {
                return HttpNotFound();
            }
            return View(exam);
        }

        // POST: Teacher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Account account = db.Account.Find(id);
            db.Account.Remove(account);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Teacher/Delete/5
        [HttpPost, ActionName("DeleteExam")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteExamConfirmed(int id)
        {
            Exam exam = db.Exam.Find(id);
            db.Exam.Remove(exam);
            db.SaveChanges();
            return RedirectToAction("ExamPage");
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
