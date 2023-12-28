using System.ComponentModel.DataAnnotations;

namespace DataLayer.Classes
{
    public class DtoWorkedHours
    {
        public DtoWorkedHours(string employeeCode, string clientCode, DateTime start, DateTime end, string notes)
        {
            emp_code = employeeCode;
            clt_code = clientCode;
            start_time = start;
            end_time = end;
            notes = notes;
        }
        public DtoWorkedHours()
        {

        }
        [Key]
        public int entry_id { get; set; }
        public string emp_code { get; set; }
        public string clt_code { get; set; }
        public int week { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public string notes { get; set; }

        public DateTime start_time { get; set; }

        public DateTime end_time { get; set; }

        public double worked_hours { get; set; }
    }
}
