using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GreenGo.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin/Login
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            return View();
        }
        // GET: Admin/Destinations
        public ActionResult Destinations()
        {
            return View();
        }
        // GET: Admin/Users
        public ActionResult Users()
        {
            return View();
        }
        // GET: Admin/Transactions
        public ActionResult Transactions()
        {
            return View();
        }
        // GET: Admin/Reservations
        public ActionResult Reservations()
        {
            return View();
        }
        // GET: Admin/Categories
        public ActionResult Categories()
        {
            return View();
        }
        public ActionResult Profile()
        {
            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }
    }
}