using System.Web.Mvc;

namespace GreenGo.Controllers
{
    public class CategoriesController : Controller
    {
        public ActionResult Index(int page = 1)
        {
            // You can later replace this with real data from your database.
            ViewBag.Page = page;
            return View();
        }

        public ActionResult Page(int id)
        {
            // id = page number (e.g., 2, 3, 4)
            ViewBag.Page = id;
            return View("Index");
        }
    }
}
