namespace DataLayer.Classes
{
    public class DtoLog
    {
        public int log_id { get; set; }
        public string message { get; set; }
        public string stack { get; set; }
        public int emp_id { get; set; }
        public DateTime date_created { get; set; }
    }
}
