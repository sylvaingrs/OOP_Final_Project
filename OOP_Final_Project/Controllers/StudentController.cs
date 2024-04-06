using OOP_Final_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace OOP_Final_Project.Controllers
{
    public class StudentController : Controller
    {
        DBModels db = new DBModels();
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
            return View(account);
        }

        public ActionResult Courses(int? id)
        {
            //return View(db.Course.ToList());

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Account account = db.Account.Find(id);

            //foreach (var item in db.Course)
            //{
            //    item.Account = db.Account.Find(item.AccountId);
            //}

            //Account account = db.Account.Include(a => a.Course).FirstOrDefault(a => a.Id == id);

            Account account = db.Account.FirstOrDefault(a => a.Id == id);

            //Course course = db.Course.FirstOrDefault();

            if (account == null)
            {
                return HttpNotFound();
            }
            //return View(course);

            var cohort = db.Cohort.FirstOrDefault(x => x.Id == account.CohortId);

            if (cohort == null)
            {
                return HttpNotFound();
            }
            
            return View(cohort.Course.ToArray());
        }

        public ActionResult Exams(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Account account = db.Account.FirstOrDefault(a => a.Id == id);

            if (account == null)
            {
                return HttpNotFound();
            }

            Cohort cohort = db.Cohort.FirstOrDefault(x => x.Id == account.CohortId);

            if (cohort == null)
            {
                return HttpNotFound();
            }

            List<Exam> exams = new List<Exam>();

            foreach (Course course in cohort.Course.ToArray())
            {
                foreach (Exam exam in course.Exam.ToArray())
                {
                    exams.Add(exam);
                }
            }
            ViewBag.Exams = exams;
            return View(exams);
        }
    }
}