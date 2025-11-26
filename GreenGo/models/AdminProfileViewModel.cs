using System;
using System.ComponentModel.DataAnnotations;

namespace GreenGo.Models
{
    public class AdminProfileViewModel
    {
        public string FirstName { get; set; } = "Jenn";
        public string LastName { get; set; } = "Mercado";

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; } = new DateTime(2004, 12, 21);

        public string Email { get; set; } = "Jennmercado@gmail.com";
        public string PhoneNumber { get; set; } = "0999999999";
        public string Role { get; set; } = "Admin";
        public string Province { get; set; } = "Metro Manila";
        public string City { get; set; } = "Quezon City";
        public string PostalCode { get; set; } = "1119";
    }
}