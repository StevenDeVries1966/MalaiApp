namespace DataLayer.Classes
{
    public class DtoWorkedHoursByJobReport
    {
        public DtoWorkedHoursByJobReport(DtoClient client, List<DtoWorkedHours> lstWorkedHours, DtoJob job)
        {
            Client = client;
            Job = job;
            foreach (DtoWorkedHours wh in lstWorkedHours)
            {
                if (wh.emp_code.Equals("ES001"))
                {
                    DblEs001MinutesTotal += wh.minutes_worked;
                }
                if (wh.emp_code.Equals("AS001"))
                {
                    DblAs001MinutesTotal += wh.minutes_worked;
                }
            }
        }

        public double DblEs001MinutesTotal { get; set; }

        public string StrEs001MinutesTotal => (Convert.ToInt32(DblEs001MinutesTotal) != 0) ? AssistFormat.ConvertMinutesToString(Convert.ToInt32(DblEs001MinutesTotal)) : String.Empty;

        public double DblAs001MinutesTotal { get; set; }

        public string StrAs001MinutesTotal => (Convert.ToInt32(DblAs001MinutesTotal) != 0) ? AssistFormat.ConvertMinutesToString(Convert.ToInt32(DblAs001MinutesTotal)) : String.Empty;
        public double MinutesTotal => DblEs001MinutesTotal + DblAs001MinutesTotal;

        public string MinutesTotalString => AssistFormat.ConvertMinutesToString(Convert.ToInt32(MinutesTotal));


        public DateTime Today { get; set; }
        public DtoClient Client { get; set; }
        public DtoJob Job { get; set; }
    }
}
