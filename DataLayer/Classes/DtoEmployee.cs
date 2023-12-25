using System.ComponentModel.DataAnnotations;

namespace DataLayer.Classes
{
    public class DtoEmployee
    {
        public DtoEmployee(int id, int code, int firstName, int lastName, int email, int phone)
        {
            Id = id;
            Code = code;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
        }

        [Key]
        public int Id { get; set; }
        public int Code { get; set; }
        public int FirstName { get; set; }
        public int LastName { get; set; }
        public int Email { get; set; }
        public int Phone { get; set; }

    }
}
