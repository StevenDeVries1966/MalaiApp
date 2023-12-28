using System.ComponentModel.DataAnnotations;

namespace DataLayer.Classes
{
    public class DtoJob
    {
        public DtoJob(string name, string clientCode)
        {
            job_name = name;
            clt_code = clientCode;
        }

        public DtoJob()
        {

        }
        [Key]
        public int job_id { get; set; }

        public string job_name { get; set; }

        public string clt_code { get; set; }

    }
}
