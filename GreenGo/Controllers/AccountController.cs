using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GreenGo.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account/Register
        public ActionResult Register()
        {
            return View();
        }

        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // GET: Account/ForgotPassword
        public ActionResult ForgotPassword()
        {
            return View();
        }

        // GET: Account/Verification
        public ActionResult Verification()
        {
            return View();
        }

        // GET: Account/NewPassword
        public ActionResult NewPassword()
        {
            return View();
        }

        // GET: Account/Success
        public ActionResult Success()
        {
            return View();
        }
    }
}