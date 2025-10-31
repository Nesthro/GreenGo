using System;
using System.Web.Mvc;

namespace GreenGo.Controllers
{
    public class CitiesMPController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        // Route target for URLs like /CitiesMP/Manila
        public ActionResult City(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return RedirectToAction("Index");

            // If a specific view (e.g. Views/CitiesMP/Manila.cshtml) exists, return it.
            var viewResult = ViewEngines.Engines.FindView(ControllerContext, id, null);
            if (viewResult.View != null)
                return View(id);

            // Otherwise use a generic City.cshtml and pass the city id in ViewBag.
            ViewBag.CityId = id;
            return View("City");
        }
    }
}