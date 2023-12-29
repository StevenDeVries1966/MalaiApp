using System.ComponentModel.DataAnnotations;

namespace DataLayer.Classes
{
    public class DtoEmployee
    {
        public DtoEmployee(int id, string code, string firstName, string lastName, string email, string phone)
        {
            emp_id = id;
            emp_code = code;
            first_name = firstName;
            last_name = lastName;
            email = email;
            phone = phone;
        }

        public DtoEmployee()
        {

        }
        [Key]
        public int emp_id { get; set; }
        public string emp_code { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }

        public string display_name()
        {
            return $"{emp_code} {first_name} {last_name}";
        }
    }
}
