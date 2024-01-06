using System.ComponentModel.DataAnnotations;

namespace DataLayer.Classes
{
    public class DtoWorkedHours
    {
        public DtoWorkedHours(string employeeCode, string clientCode, string clientJobCode, DateTime start, DateTime end, string notes)
        {
            emp_code = employeeCode;
            clt_code = clientCode;
            clt_job_code = clientJobCode;
            start_time = start;
            end_time = end;
            notes = notes;
        }
        public DtoWorkedHours()
        {

        }
        [Key]
        public int entry_id { get; set; }
        public int emp_id { get; set; }
        public string emp_code { get; set; }
        public string clt_code { get; set; }
        public string clt_job_code { get; set; }
        public string job_name { get; set; }
        public int week { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public string notes { get; set; }

        public DateTime start_time { get; set; }

        public DateTime end_time { get; set; }

        public double minutes_worked { get; set; }
        public string minutes_worked_display
        {
            get
            {
                ////if (hours_worked == null) return "nvt";
                //int hours = (int)minutes_worked / 60;
                //int minutes_in_hours = (int)hours * 60;
                //int minutes = (int)minutes_worked - minutes_in_hours;
                //string strminutes = minutes < 10 ? $"0{minutes}" : Convert.ToString(minutes);
                //return $"{hours}:{strminutes}";
                return AssistFormat.ConvertMinutesToString(Convert.ToInt32(minutes_worked));
            }
            set => hours_worked_display = value;
        }

        public double hours_worked { get; set; }

        public string hours_worked_display
        {
            get
            {
                //if (hours_worked == null) return "nvt";
                int hours = (int)hours_worked;
                int minutes = (int)((hours_worked - hours) * 60);
                string strminutes = minutes < 10 ? $"0{minutes}" : Convert.ToString(minutes);
                return $"{hours}:{strminutes}";
            }
            set => hours_worked_display = value;
        }
    }
}
