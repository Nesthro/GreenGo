using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace YourNamespace.Controllers
{
    public class PaymentController : Controller
    {
        // -------------------------
        // Helpers
        // -------------------------
        private List<Place> LoadPlaces()
        {
            var path = HostingEnvironment.MapPath("~/Data/placesData.json");
            if (string.IsNullOrEmpty(path) || !System.IO.File.Exists(path))
                return new List<Place>();

            var json = System.IO.File.ReadAllText(path);
            var js = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
            var root = js.Deserialize<PlacesRoot>(json);
            return root?.Places ?? new List<Place>();
        }

        private decimal ParsePrice(string priceStr)
        {
            if (string.IsNullOrWhiteSpace(priceStr)) return 0m;
            var m = Regex.Match(priceStr, @"\\d+(\\.\\d+)?");
            if (!m.Success) return 0m;
            decimal.TryParse(m.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var val);
            return val;
        }

        private decimal ComputeTotalFromCartJson(string cartJson)
        {
            if (string.IsNullOrEmpty(cartJson)) return 0m;
            try
            {
                var js = new JavaScriptSerializer();
                var obj = js.DeserializeObject(cartJson) as Dictionary<string, object>;
                if (obj == null) return 0m;

                decimal total = 0m;

                foreach (var key in new[] { "adult", "youth", "child" })
                {
                    if (obj.ContainsKey(key) && obj[key] is Dictionary<string, object> ticket)
                    {
                        var qty = 0;
                        var price = 0m;
                        if (ticket.ContainsKey("qty")) int.TryParse(ticket["qty"].ToString(), out qty);
                        if (ticket.ContainsKey("price")) decimal.TryParse(ticket["price"].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out price);
                        total += qty * price;
                    }
                }

                if (obj.ContainsKey("activities") && obj["activities"] is Dictionary<string, object> acts)
                {
                    foreach (var kv in acts)
                    {
                        if (kv.Value is Dictionary<string, object> act)
                        {
                            var qty = 0;
                            var price = 0m;
                            if (act.ContainsKey("qty")) int.TryParse(act["qty"].ToString(), out qty);
                            if (act.ContainsKey("price")) decimal.TryParse(act["price"].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out price);
                            total += qty * price;
                        }
                    }
                }

                return total;
            }
            catch
            {
                return 0m;
            }
        }

        // -------------------------
        // GET: Payment/Booking
        // -------------------------
        public ActionResult Booking(int? id, string placeName = null)
        {
            if (id != null)
            {
                var sessionBooking = Session[$"Booking_{id.Value}"] as BookingViewModel;
                if (sessionBooking != null)
                    return View("Booking", sessionBooking);
            }

            if (!string.IsNullOrEmpty(placeName))
            {
                var places = LoadPlaces();
                var place = places.FirstOrDefault(p => string.Equals(p.Name, placeName, StringComparison.OrdinalIgnoreCase));
                if (place != null)
                {
                    var model = new BookingViewModel
                    {
                        BookingId = new Random().Next(100000, 999999),
                        MuseumName = place.Name,
                        TicketType = place.Type + " - General",
                        AvailableDate = DateTime.Now.AddDays(7),
                        Price = ParsePrice(place.PriceAdult)
                    };

                    Session[$"Place_{model.BookingId}"] = place;
                    Session[$"Booking_{model.BookingId}"] = model;

                    return View("Booking", model);
                }
            }

            var fallback = new BookingViewModel
            {
                BookingId = id ?? 1,
                MuseumName = "Unknown",
                TicketType = "Single Ticket",
                AvailableDate = DateTime.Now.AddDays(7),
                Price = 0m
            };
            return View("Booking", fallback);
        }

        // -------------------------
        // POST: Payment/Booking
        // -------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Booking(BookingViewModel model, string CartJson = null)
        {
            if (model == null)
            {
                ModelState.AddModelError("", "Invalid booking data.");
                return View("Booking", model);
            }

            var places = LoadPlaces();
            var place = places.FirstOrDefault(p => string.Equals(p.Name, model.MuseumName, StringComparison.OrdinalIgnoreCase));

            if (place != null)
            {
                if (model.Price <= 0m)
                    model.Price = ParsePrice(place.PriceAdult);
                if (string.IsNullOrEmpty(model.TicketType))
                    model.TicketType = place.Type + " - General";

                Session[$"Place_{model.BookingId}"] = place;
            }

            if (!string.IsNullOrEmpty(CartJson))
            {
                Session[$"Cart_{model.BookingId}"] = CartJson;
                var computed = ComputeTotalFromCartJson(CartJson);
                if (computed > 0m) model.Price = computed;
            }

            Session[$"Booking_{model.BookingId}"] = model;

            return RedirectToAction("EnterInfo", new { bookingId = model.BookingId });
        }

        // -------------------------
        // GET: Payment/EnterInfo
        // -------------------------
        public ActionResult EnterInfo(int? bookingId)
        {
            if (bookingId == null)
                bookingId = 999999;

            var booking = Session[$"Booking_{bookingId.Value}"] as BookingViewModel;
            var place = Session[$"Place_{bookingId.Value}"] as Place;

            var model = new EnterInfoViewModel
            {
                BookingId = bookingId.Value,
                MuseumName = booking?.MuseumName ?? place?.Name ?? "Unknown",
                TicketType = booking?.TicketType ?? (place != null ? place.Type + " - General" : "Single Ticket")
            };

            return View("EnterInfo", model);
        }

        // -------------------------
        // POST: Payment/EnterInfo
        // -------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EnterInfo(EnterInfoViewModel model)
        {
            if (!ModelState.IsValid)
                return View("EnterInfo", model);

            Session[$"Contact_{model.BookingId}"] = model;

            var booking = Session[$"Booking_{model.BookingId}"] as BookingViewModel;
            if (booking == null)
            {
                booking = new BookingViewModel
                {
                    BookingId = model.BookingId,
                    MuseumName = model.MuseumName,
                    TicketType = model.TicketType,
                    AvailableDate = DateTime.Now.AddDays(7),
                    Price = 0m
                };
                Session[$"Booking_{booking.BookingId}"] = booking;
            }

            return RedirectToAction("Verification", new { bookingId = model.BookingId });
        }

        // -------------------------
        // GET: Payment/Verification
        // -------------------------
        public ActionResult Verification(int? bookingId)
        {
            // 🔧 Ensure bookingId is retrieved properly
            if (bookingId == null)
            {
                // Try to recover last booking ID from session
                var lastBooking = Session.Keys.Cast<string>()
                    .Where(k => k.StartsWith("Booking_"))
                    .LastOrDefault();

                if (lastBooking != null)
                    bookingId = int.Parse(lastBooking.Replace("Booking_", ""));
                else
                    return View("Verification", new VerificationViewModel
                    {
                        BookingId = 0
                    });
            }

            var booking = Session[$"Booking_{bookingId.Value}"] as BookingViewModel;
            var contact = Session[$"Contact_{bookingId.Value}"] as EnterInfoViewModel;

            var model = new VerificationViewModel
            {
                BookingId = bookingId.Value
            };

            ViewBag.Booking = booking;
            ViewBag.Contact = contact;

            return View("Verification", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Verification(VerificationViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Verification", model);

            // Example validation for demo
            var code = $"{model.Code1}{model.Code2}{model.Code3}{model.Code4}";
            if (code == "1234")
                return RedirectToAction("Success", new { bookingId = model.BookingId });

            ModelState.AddModelError("", "Invalid verification code.");
            return View("Verification", model);
        }

        // -------------------------
        // GET: Payment/Success
        // -------------------------
        public ActionResult Success(int? bookingId)
        {
            if (bookingId == null) return RedirectToAction("Booking");

            ViewBag.Booking = Session[$"Booking_{bookingId.Value}"] as BookingViewModel;
            ViewBag.Contact = Session[$"Contact_{bookingId.Value}"] as EnterInfoViewModel;
            ViewBag.Verified = Session[$"Verified_{bookingId.Value}"] as bool?;

            return View("Success");
        }
    }

    // -------------------------
    // DTOs & ViewModels
    // -------------------------
    public class PlacesRoot { public List<Place> Places { get; set; } }

    public class Place
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string GoogleMapsEmbedUrl { get; set; }
        public string PriceAdult { get; set; }
        public string PriceLabel { get; set; }
        public List<string> Highlights { get; set; }
        public string HoursDaily { get; set; }
        public string HoursNotice { get; set; }
        public List<Activity> Activities { get; set; }
        public List<string> GalleryImages { get; set; }
        public List<string> RecommendedPlaces { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
    }

    public class Activity
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }

    public class BookingViewModel
    {
        public int BookingId { get; set; }
        public string MuseumName { get; set; }
        public string TicketType { get; set; }
        public DateTime AvailableDate { get; set; }
        public decimal Price { get; set; }
    }

    public class EnterInfoViewModel
    {
        public int BookingId { get; set; }
        public string MuseumName { get; set; }
        public string TicketType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneCountryCode { get; set; } = "+63";
        public string PhoneNumber { get; set; }
    }

    public class PaymentViewModel
    {
        public int BookingId { get; set; }
        public string MuseumName { get; set; }
        public string TicketType { get; set; }
        public DateTime PlannedDate { get; set; }
        public string Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string CardNumber { get; set; }
        public string ExpirationDate { get; set; }
        public string SecurityCode { get; set; }
        public bool SaveCardDetails { get; set; }
    }

    public class VerificationViewModel
    {
        public int BookingId { get; set; }
        public string Code1 { get; set; }
        public string Code2 { get; set; }
        public string Code3 { get; set; }
        public string Code4 { get; set; }
    }
}
