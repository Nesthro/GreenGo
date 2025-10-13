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

        public ActionResult AnthropologyMuseum()
        {
            return View("~/Views/MuseumDesc/AnthropologyMuseum.cshtml");
        }

        public ActionResult MuseoNgPagAsa()
        {
            return View("~/Views/MuseumDesc/MuseoNgPagAsa.cshtml");
        }

        public ActionResult TheMindMuseum()
        {
            return View("~/Views/MuseumDesc/TheMindMuseum.cshtml");
        }

        public ActionResult DessertMuseum()
        {
            return View("~/Views/MuseumDesc/DessertMuseum.cshtml");
        }

        public ActionResult ManilaClockTowerMuseum()
        {
            return View("~/Views/MuseumDesc/ManilaClockTowerMuseum.cshtml");
        }
            public ActionResult ArtInIsland()
        {
            return View("~/Views/MuseumDesc/ArtInIsland.cshtml");
        }


    }
}
