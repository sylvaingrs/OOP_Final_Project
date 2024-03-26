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
    }
}