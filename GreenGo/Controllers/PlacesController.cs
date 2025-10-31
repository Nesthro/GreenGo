using GreenGo.Models;
using Newtonsoft.Json; // Make sure you have this using statement (or System.Web.Script.Serialization)
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GreenGo.Controllers
{
    public class PlacesController : Controller
    {
        // --- ADD THIS HELPER FUNCTION TO LOAD YOUR DATA ---
        private List<Place> LoadPlacesData()
        {
            try
            {
                string filePath = Server.MapPath("~/Data/placesData.json");
                if (System.IO.File.Exists(filePath))
                {
                    string jsonData = System.IO.File.ReadAllText(filePath);
                    AllPlaces allPlaces = JsonConvert.DeserializeObject<AllPlaces>(jsonData);
                    return allPlaces.Places ?? new List<Place>();
                }
            }
            catch (Exception ex)
            {
                // Log the error
                System.Diagnostics.Debug.WriteLine("Error loading placesData.json: " + ex.Message);
            }
            return new List<Place>(); // Return an empty list on failure
        }

        // --- THIS IS THE ACTION THAT IS CAUSING THE 404 ERROR ---
        // GET: Places/CitySummary
        public ActionResult CitySummary(string city)
        {
            if (string.IsNullOrEmpty(city))
            {
                return HttpNotFound("City not specified.");
            }

            var allPlaces = LoadPlacesData();

            // Find all places that match the city name (case-insensitive)
            var cityPlaces = allPlaces
                .Where(p => p.City.Equals(city, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!cityPlaces.Any())
            {
                // This is the error you are seeing
                return HttpNotFound("No places found for " + city);
            }

            ViewBag.CityName = city; // To display "Explore all places in Manila City"
            return View(cityPlaces); // Pass the filtered list to the CitySummary.cshtml view
        }

        // --- THIS ACTION LOADS YOUR DETAIL PAGE ---
        // GET: Places/Details/5
        public ActionResult Details(int id, string city, string type)
        {
            var allPlaces = LoadPlacesData();

            // Find the specific place by its Id, City, and Type
            var place = allPlaces
                .FirstOrDefault(p => p.Id == id &&
                                     p.City.Equals(city, StringComparison.OrdinalIgnoreCase) &&
                                     p.Type.Equals(type, StringComparison.OrdinalIgnoreCase));

            if (place == null)
            {
                return HttpNotFound("Place not found.");
            }

            return View(place); // Pass the single Place object to the Details.cshtml view
        }
    }
}

