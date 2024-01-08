using System.ComponentModel.DataAnnotations;

namespace DataLayer.Classes
{
    public class DtoClient
    {
        public DtoClient(string code, string name, string address, string postalCode, string city, string country, string email, string phone, double rateES001, double rateAS001, string reportType)
        {
            clt_code = code;
            clt_name = name;
            address = address;
            postalCode = postalCode;
            city = city;
            country = country;
            email = email;
            phone = phone;
            rate_ES001 = rateES001;
            rate_AS001 = rateAS001;
            this.report_type = reportType;
        }
        public DtoClient()
        {

        }
        [Key]
        public string clt_code { get; set; }
        public string clt_name { get; set; }
        public string address { get; set; }
        public string postalcode { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public double rate_ES001 { get; set; }
        public double rate_AS001 { get; set; }
        public double retainer_ES001 { get; set; }
        public double retainer_AS001 { get; set; }
        public string report_type { get; set; }

    }
}
