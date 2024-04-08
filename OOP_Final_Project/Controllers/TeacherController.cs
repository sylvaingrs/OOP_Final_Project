﻿//Roxane Seibel 74527 - Jon Gouspy 74538 - Sylvain Gross 74525 - Leith Chakroun 74529
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using OOP_Final_Project.Class;
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

        public ActionResult ExamPage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var cohort = db.Cohort.Find(id);
            

            List<Exam> exams = new List<Exam>();

            foreach (Course course in cohort.Course.ToArray())
            {
                foreach (Exam exam in course.Exam.ToArray())
                {
                    exams.Add(exam);
                }
            }

            ViewBag.Er = exams;
            return View(exams);
        }

        public ActionResult AddGrade(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var exam = db.Exam.Find(id);

            if (exam == null)
            {
                return HttpNotFound();
            }

            Course course = db.Course.Find(exam.CourseId);

            List<Cohort> cohorts = new List<Cohort>();

            foreach (Cohort cohort in course.Cohort)
            {
                cohorts.Add(db.Cohort.FirstOrDefault(c => c.Id == cohort.Id));
            }

            List<Account> students = db.Account.Where(c => c.AccountType == (short)Account.EAccountType.Student).ToList();

            List<Account> cohortStudents = new List<Account>();

            foreach (Account student in students)
            {
                if (cohorts.Any(c => c.Id == student.CohortId))
                {
                    cohortStudents.Add(student);
                }
            }

            ViewBag.AccountList = new SelectList(cohortStudents, "Id", "FisrtName");

            Result result = new Result();
            result.ExamId = exam.Id;

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddGrade([Bind(Include = "Id,AccountId,ExamId,Grade")] Result result)
        {
            if (ModelState.IsValid)
            {
                if (db.Result.Any(r => r.Id == result.Id))
                {
                    db.Entry(result).State = EntityState.Modified;
                }
                else
                {
                    result.Exam = db.Exam.FirstOrDefault(e => e.Id == result.ExamId);
                    result.Account = db.Account.FirstOrDefault(a => a.Id == result.AccountId);
                    db.Result.Add(result);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Result = new SelectList(db.Result, "Id", "ResultId", result);
            return View(result);
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

            Dictionary<Course, double> grades = ResultCalulation.CalculateAveragePerCourse(account.Cohort, account.Id);

            ViewBag.CohortId = account.CohortId;
            ViewBag.Grades = grades;
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
            var courses = db.Course.ToList();

            ViewBag.CourseList = new SelectList(courses, "Id", "CourseName");

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

            var courses = db.Course.ToList();

            ViewBag.CourseList = new SelectList(courses, "Id", "CourseName");
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

            var courses = db.Course.ToList();

            ViewBag.CourseList = new SelectList(courses, "Id", "CourseName");

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
        public ActionResult EditExam([Bind(Include = "Id,Date,CourseId,Coefficient")]Exam exam, int? courseId)
        {
            if (courseId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ModelState.IsValid)
            {
                exam.CourseId = (int)courseId;

                db.Entry(exam).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var courses = db.Course.ToList();

            ViewBag.CourseList = new SelectList(courses, "Id", "CourseName");
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
