using System.Collections.Generic;

namespace GreenGo.Models
{
    // The main Place model, containing lists of complex objects
    public class Place
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string GoogleMapsEmbedUrl { get; set; }
        public string PriceAdult { get; set; } // e.g., "₱750" or "Free"
        public string PriceLabel { get; set; }
        public string HoursDaily { get; set; }
        public string HoursNotice { get; set; }
        public List<string> Highlights { get; set; }
        public List<string> IncludedServices { get; set; }

        // *** CONTACT INFO FIX: ADDED MISSING PROPERTIES ***
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        // **********************************************

        public List<Activity> Activities { get; set; }
        public List<string> GalleryImages { get; set; }
    }

    // Nested class that holds the structure for individual activities (must be defined or included)
    public class Activity
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }

    // Container for JSON deserialization
    public class AllPlaces
    {
        public List<Place> Places { get; set; }
    }
}
