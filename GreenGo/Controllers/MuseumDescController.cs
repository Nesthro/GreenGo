using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GreenGo.Controllers
{
    public class MuseumDescController : Controller
    {
        // GET: MuseumDesc
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NationalMuseum()
        {
            return View("~/Views/MuseumDesc/NationalMuseum.cshtml");
        }
        public ActionResult YuchengcoMuseum()
        {
            return View("~/Views/MuseumDesc/YuchengcoMuseum.cshtml");
        }

    }
}