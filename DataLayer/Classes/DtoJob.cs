using System.ComponentModel.DataAnnotations;

namespace DataLayer.Classes
{
    public class DtoJob
    {
        public DtoJob(string name, string clientCode)
        {
            Name = name;
            ClientCode = clientCode;
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string ClientCode { get; set; }

    }
}
