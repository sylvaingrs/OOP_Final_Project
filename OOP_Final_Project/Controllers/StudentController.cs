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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Account account = db.Account.Find(id);

            Account account = db.Account.Include(a => a.Course).FirstOrDefault(a => a.Id == id);

            if (account == null)
            {
                return HttpNotFound();
            }

            return View(account.Course.ToList());
            //return View(db.Course.ToList());
        }

        public ActionResult Exams()
        {
            return View(db.Exam.ToList());
        }
    }
}