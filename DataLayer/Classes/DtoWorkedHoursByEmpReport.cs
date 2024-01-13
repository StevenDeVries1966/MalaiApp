namespace DataLayer.Classes
{
    public class DtoWorkedHoursByEmpReport
    {
        public DtoWorkedHoursByEmpReport(DtoClient client, List<DtoWorkedHours> lstWorkedHours, DateTime today)
        {
            this.Today = today;
            Client = client;
            foreach (DtoWorkedHours wh in lstWorkedHours)
            {
                if (client.report_type.Equals("Hrs_B", StringComparison.CurrentCultureIgnoreCase))
                {
                    // PayRoll hrs in own HR column
                    if (!wh.job_name.Contains("Payroll", StringComparison.CurrentCultureIgnoreCase))
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
                    else
                    {
                        DblHrMinutesTotal += wh.minutes_worked;
                    }
                }
                else if (client.report_type.Equals("Hrs_A", StringComparison.CurrentCultureIgnoreCase) || client.report_type.Equals("Hrs_C", StringComparison.CurrentCultureIgnoreCase))
                {
                    // add PayRoll hrs to total of emp hrs
                    if (wh.emp_code.Equals("ES001"))
                    {
                        DblEs001MinutesTotal += wh.minutes_worked;
                    }
                    if (wh.emp_code.Equals("AS001"))
                    {
                        DblAs001MinutesTotal += wh.minutes_worked;
                    }
                }
                else if (client.report_type.Equals("Ret", StringComparison.CurrentCultureIgnoreCase))
                {
                    // ignore PayRoll hrs
                    if (!wh.job_name.Contains("Payroll", StringComparison.CurrentCultureIgnoreCase))
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

                if (!jobs.Contains(wh.job_name))
                {
                    jobs.Add(wh.job_name);
                }
            }
        }
        public DateTime Today { get; set; }
        public DtoClient Client { get; set; }

        public double DblEs001MinutesTotal { get; set; }

        public string StrEs001MinutesTotal => (Convert.ToInt32(DblEs001MinutesTotal) != 0) ? AssistFormat.ConvertMinutesToString(Convert.ToInt32(DblEs001MinutesTotal)) : String.Empty;

        public double DblAs001MinutesTotal { get; set; }

        public string StrAs001MinutesTotal => (Convert.ToInt32(DblAs001MinutesTotal) != 0) ? AssistFormat.ConvertMinutesToString(Convert.ToInt32(DblAs001MinutesTotal)) : String.Empty;

        public double DblHrMinutesTotal { get; set; }

        public string StrHrMinutesTotal => (Convert.ToInt32(DblHrMinutesTotal) != 0) ? AssistFormat.ConvertMinutesToString(Convert.ToInt32(DblHrMinutesTotal)) : String.Empty;

        public double MinutesTotal => DblEs001MinutesTotal + DblAs001MinutesTotal + DblHrMinutesTotal;

        public string MinutesTotalString => AssistFormat.ConvertMinutesToString(Convert.ToInt32(MinutesTotal));

        public double ChargeEs001 => Math.Round((DblEs001MinutesTotal / 60) * Client.rate_ES001, 2);
        public double ChargeAs001 => Math.Round((DblAs001MinutesTotal / 60) * Client.rate_AS001, 2);
        public double ChargeHr => Math.Round((DblHrMinutesTotal / 60) * Client.rate_ES001, 2); //Todo : Is er een appart HR rate

        public double Charge => Math.Round(ChargeEs001 + ChargeAs001, 2);

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
