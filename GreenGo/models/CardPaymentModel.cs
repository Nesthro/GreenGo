using System.ComponentModel.DataAnnotations;

namespace GreenGo.Models
{
    public class CardPaymentModel
    {
        [Required]
        public string CardNumber { get; set; }

        [Required]
        public string CardName { get; set; }

        [Required]
        public string ExpirationDate { get; set; }

        [Required]
        public string SecurityCode { get; set; }
    }
}