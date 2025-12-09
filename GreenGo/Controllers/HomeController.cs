using GreenGo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization; // Required for JSON deserialization

namespace GreenGo.Controllers
{
    public class HomeController : Controller
    {
        // --- ADD THIS HELPER FUNCTION TO LOAD YOUR DATA ---
        private List<Place> LoadPlacesData()
        {
            try
            {
                // Correct path to your JSON file
                string path = Server.MapPath("~/Data/placesData.json");

                // Check if file exists before reading
                if (!System.IO.File.Exists(path))
                {
                    Console.WriteLine("Error: placesData.json not found at " + path);
                    return new List<Place>(); // Return empty list
                }

                string jsonData = System.IO.File.ReadAllText(path);
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                // Deserialize the top-level object which contains the "Places" list
                AllPlaces allPlaces = serializer.Deserialize<AllPlaces>(jsonData);

                if (allPlaces != null && allPlaces.Places != null)
                {
                    return allPlaces.Places;
                }
            }
            catch (Exception ex)
            {
                // Log the error 
                Console.WriteLine("JSON Deserialization Error: " + ex.Message);
            }
            return new List<Place>(); // Return an empty list on failure
        }

        // --- UPDATE YOUR INDEX ACTION LIKE THIS ---
        // GET: Home/Index (Welcome Page)
        public ActionResult Index()
        {
            // Load all places and pass them to the Index view
            List<Place> allPlaces = LoadPlacesData();
            return View(allPlaces);
        }

        // GET: Home/Home (Main Homepage after login)
        public ActionResult Home()
        {
            // Also load data for the main Home view
            List<Place> allPlaces = LoadPlacesData();
            return View(allPlaces);
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

        public ActionResult PrivacyPolicy()
        {
            return View();
        }

        public ActionResult GeneralTerms()
        {
            return View();
        }

        public ActionResult Tips()
        {
            return View();
        }

        public ActionResult MuseumTopAttractions()
        {
            // Load *all* places from your JSON
            List<Place> allPlaces = LoadPlacesData();

            // Pass the entire list to your new view.
            // The view itself will handle the filtering.
            return View(allPlaces);
        }

        public ActionResult ParksTopAttractions()
        {
            // You might want to filter this later
            List<Place> allPlaces = LoadPlacesData();
            var parks = allPlaces.Where(p => p.Type == "Park").ToList();
            // Assuming you have a view named "CitySummary" to display lists
            return View("~/Views/Places/CitySummary.cshtml", parks);
        }

        public ActionResult EcoFriendly()
        {
          
            return View();
        }

        public ActionResult Cities()
        {
            return View();
        }
    }
}

