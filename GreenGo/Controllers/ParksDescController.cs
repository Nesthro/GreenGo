using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GreenGo.Controllers
{
    public class ParksDescController : Controller
    {
        // GET: Parks
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LaMesaEcoPark()
        {
            return View();
        }


        public ActionResult MarikinaRiverPark()
        {
            return View();
        }

        public ActionResult OrtigasPark()
        {
            return View();
        }

        public ActionResult WashingtonSycipPark()
        {
            return View();
        }

        public ActionResult ValenzuelaCityPeoplesPark()
        {
            return View();
        }

        public ActionResult NinoyAquinoParks()
        {
            return View();
        }

        public ActionResult PasigRAVEPark()
        {
            return View();
        }

        public ActionResult KalikasanGardenPark()
        {
            return View();
        }


        public ActionResult Arroceros()
        {
            return View("~/Views/ParksDesc/Arroceros.cshtml");
        }

        public ActionResult Riverbanks()
        {
            return View("~/Views/ParksDesc/Riverbanks.cshtml");
        }
    }
}