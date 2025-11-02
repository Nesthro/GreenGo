using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GreenGo.Models;

namespace GreenGo.Controllers
{
    public class UserProfileController : Controller
    {
        public ActionResult Profile()
        {
            return View();
        }
        public ActionResult Bookings()
        {
            return View();
        }

        public ActionResult Reviews()
        {
            return View();
        }

        public ActionResult Bookmarks()
        {
            return View();
        }

        public ActionResult Settings()
        {
            return View();
        }

        public ActionResult Help()
        {
            return View();
        }
    }
}