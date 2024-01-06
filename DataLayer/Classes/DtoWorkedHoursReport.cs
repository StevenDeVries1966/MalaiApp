namespace DataLayer.Classes
{
    public class DtoWorkedHoursReport
    {
        public DtoWorkedHoursReport(List<DtoWorkedHours> lstWorkedHours, DateTime today)
        {
            this.today = today;
            foreach (DtoWorkedHours wh in lstWorkedHours)
            {
                if (wh.emp_code.Equals("ES001"))
                {
                    //esther_hours_total += wh.hours_worked;
                    esther_minutes_total += wh.minutes_worked;
                }
                if (wh.emp_code.Equals("AS001"))
                {
                    //aisha_hours_total += wh.hours_worked;
                    aisha_minutes_total += wh.minutes_worked;
                }

                if (!jobs.Contains(wh.job_name))
                {
                    jobs.Add(wh.job_name);
                }
            }
        }
        public DateTime today { get; set; }

        //public double esther_hours_total { get; set; }

        //public string esther_hours_total_string => AssistFormat.ConvertHoursToString(esther_hours_total);

        public double esther_minutes_total { get; set; }

        public string esther_minutes_total_string => AssistFormat.ConvertMinutesToString(Convert.ToInt32(esther_minutes_total));

        //public double aisha_hours_total { get; set; }

        //public string aisha_hours_total_string => AssistFormat.ConvertHoursToString(aisha_hours_total);

        public double aisha_minutes_total { get; set; }

        public string aisha_minutes_total_string => AssistFormat.ConvertMinutesToString(Convert.ToInt32(aisha_minutes_total));

        //public double hours_total => esther_hours_total + aisha_hours_total;
        //public string hours_total_string => AssistFormat.ConvertHoursToString(hours_total);

        public double minutes_total => esther_minutes_total + aisha_minutes_total;

        public string minutes_total_string => AssistFormat.ConvertMinutesToString(Convert.ToInt32(minutes_total));

        public double charge => Math.Round((minutes_total / 60) * 35, 2);

        public List<string> jobs = new();
        public string job_total
        {
            get
            {
                string _job_total = default;
                foreach (string job in jobs)
                {
                    _job_total += job + " / ";
                }
                _job_total = _job_total.Remove(_job_total.Length - 3);
                return _job_total;
            }
        }
    }
}
