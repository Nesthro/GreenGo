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