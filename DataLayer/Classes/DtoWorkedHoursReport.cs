namespace DataLayer.Classes
{
    public class DtoWorkedHoursReport
    {
        public DtoWorkedHoursReport(List<DtoWorkedHours> lstWorkedHours, DateTime today)
        {
            this.Today = today;
            foreach (DtoWorkedHours wh in lstWorkedHours)
            {
                if (wh.emp_code.Equals("ES001"))
                {
                    Es001MinutesTotal += wh.minutes_worked;
                }
                if (wh.emp_code.Equals("AS001"))
                {
                    As001MinutesTotal += wh.minutes_worked;
                }

                if (!jobs.Contains(wh.job_name))
                {
                    jobs.Add(wh.job_name);
                }
            }
        }
        public DateTime Today { get; set; }

        public double Es001MinutesTotal { get; set; }

        public string Es001MinutesTotalString => AssistFormat.ConvertMinutesToString(Convert.ToInt32(Es001MinutesTotal));

        public double As001MinutesTotal { get; set; }

        public string As001MinutesTotalString => AssistFormat.ConvertMinutesToString(Convert.ToInt32(As001MinutesTotal));

        public double MinutesTotal => Es001MinutesTotal + As001MinutesTotal;

        public string MinutesTotalString => AssistFormat.ConvertMinutesToString(Convert.ToInt32(MinutesTotal));

        public double Charge => Math.Round((MinutesTotal / 60) * 35, 2);

        public readonly List<string> jobs = new();
        public string JobTotal
        {
            get
            {
                string jobTotal = default;
                foreach (string job in jobs)
                {
                    jobTotal += job + " / ";
                }
                jobTotal = jobTotal?.Remove(jobTotal.Length - 3);
                return jobTotal;
            }
        }
    }
}
