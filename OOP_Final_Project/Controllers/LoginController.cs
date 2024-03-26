using OOP_Final_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OOP_Final_Project.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Authorise(Account ac)
        {
            using (DBModels db = new DBModels())
            {
                var userDetail = db.Account.Where(x => x.FisrtName == ac.FisrtName && x.LastName == ac.LastName && x.Password == ac.Password).FirstOrDefault();
                if (userDetail == null)
                {
                    ac.LoginErrorMessage = "Wrong Identification or Passeword";
                    return View("Index", ac);
                }
                else
                {
                    Session["id"] = userDetail.Id;
                    Session["email"] = userDetail.Email;
                    Session["name"] = userDetail.FisrtName;
                    Session["lastName"] = userDetail.LastName;
                    Session["accountType"] = userDetail.AccountType;
                    return RedirectToAction("Index", "Student", userDetail);
                }
            }

        }

        public ActionResult LogOut()
        {
            int userId = (int)Session["userID"];
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    }
}