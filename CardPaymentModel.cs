csharp GreenGo\models\CardPaymentModel.cs
using System.ComponentModel.DataAnnotations;

namespace GreenGo.Models
{
    // Minimal card payment model so PaymentController compiles.
    // Replace/extend with real gateway fields and server-side validation as needed.
    public class CardPaymentModel
    {
        [Required]
        [Display(Name = "Cardholder Name")]
        public string CardHolderName { get; set; }

        [Required]
        [Display(Name = "Card Number")]
        [CreditCard]
        public string CardNumber { get; set; }

        [Required]
        [Range(1, 12)]
        public int ExpiryMonth { get; set; }

        [Required]
        [Range(2020, 2100)]
        public int ExpiryYear { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 3)]
        public string Cvv { get; set; }

        // Optional: include amount if you want server-side check
        public decimal Amount { get; set; }
    }
}