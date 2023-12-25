namespace DataLayer.Classes
{
    public class DtoWorkedHours
    {
        public DtoWorkedHours(string employeeCode, string clientCode, DateTime start, DateTime end, string notes)
        {
            EmployeeCode = employeeCode;
            ClientCode = clientCode;
            Start = start;
            End = end;
            Notes = notes;
        }

        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string ClientCode { get; set; }
        public int Week { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string Notes { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public double WorkedHours { get; set; }
    }
}
