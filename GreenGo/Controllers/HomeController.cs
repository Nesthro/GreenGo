using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GreenGo.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home/Index (Welcome Page)
        public ActionResult Index()
        {
            return View();
        }

        // GET: Home/Home (Main Homepage after login)
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Categories()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Tips()
        {
            return View();
        }

        public ActionResult MuseumTopAttractions()
        {
            return View();
        }

        public ActionResult ParksTopAttractions()
        {
            return View();
        }

        public ActionResult Cities()
        {
            return View();
        }


    }
}