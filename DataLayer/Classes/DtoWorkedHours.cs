﻿using System.ComponentModel.DataAnnotations;

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
        public int emp_id { get; set; }
        public string emp_code { get; set; }
        public string clt_code { get; set; }
        public int week { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public string notes { get; set; }

        public DateTime start_time { get; set; }

        public DateTime end_time { get; set; }

        public double hours_worked { get; set; }

        public string hours_worked_display
        {
            get
            {
                //if (hours_worked == null) return "nvt";
                int hours = (int)hours_worked;
                int minutes = (int)((hours_worked - hours) * 60);
                string strminutes = hours < 10 ? $"0{minutes}" : Convert.ToString(minutes);
                return $"{hours}:{strminutes}";
            }
        }
    }
}