using System.ComponentModel.DataAnnotations;

namespace DataLayer.Classes
{
    public class DtoClient
    {
        public DtoClient(string code, string name, string address, string postalCode, string city, string country, string email, string phone)
        {
            Code = code;
            Name = name;
            Address = address;
            PostalCode = postalCode;
            City = city;
            Country = country;
            Email = email;
            Phone = phone;
        }

        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

    }
}
