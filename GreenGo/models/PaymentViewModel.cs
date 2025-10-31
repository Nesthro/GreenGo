using System;
using System.ComponentModel.DataAnnotations;

namespace GreenGo.Models
{
    public class PaymentViewModel
    {
        public string PlaceName { get; set; }
        public decimal TotalFee { get; set; }
        public string CartDataJson { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string PhoneCountryCode { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}