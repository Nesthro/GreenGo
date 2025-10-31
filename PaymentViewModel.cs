using System.ComponentModel.DataAnnotations;

namespace GreenGo.Models
{
    public class PaymentViewModel
    {
        // Payment / place info
        public string PlaceName { get; set; }

        public decimal TotalFee { get; set; }

        // JSON representation of cart (used by the view)
        public string CartDataJson { get; set; }

        // User/contact info used by PaymentController.ProcessPayment
        [Required(ErrorMessage = "First name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        // Optional phone fields; controller stores them to session
        public string PhoneCountryCode { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }
    }
}