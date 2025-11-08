using DataLayer.Classes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WpfUI.Helpers;

namespace WpfUI.Data
{
    public interface IMalaiDataProvider
    {
        Task<MalaiContext?> GetDataContextAsync();
        Task<List<DtoEmployee>?> GetEmployeesAsync();
        List<DtoEmployee> GetEmployees();
        Task<List<DtoClient>?> GetClientsAsync();
        List<DtoClient> GetClients();
        Task<List<DtoJob>?> GetJobsAsync();
        List<DtoJob> GetJobs();
        Task<List<DtoWorkedHours>?> GetWorkedHoursAsync();
        List<DtoWorkedHours> GetWorkedHours();
    }

    public class MalaiDataProvider : IMalaiDataProvider
    {
        public async Task<MalaiContext?> GetDataContextAsync()
        {
            await Task.Delay(1);
            MalaiContext ctx = new MalaiContext(Globals.ConnectionString, true);
            ctx.GetAllWorkedHours(DateTime.ParseExact(Globals.SelectedMonth, "MMMM", System.Globalization.CultureInfo.CurrentCulture).Month, Globals.SelectedYear);
            ;
            return ctx;
        }
        public async Task<List<DtoEmployee>?> GetEmployeesAsync()
        {
            await Task.Delay(1);
            return GetEmployees();
        }

        public List<DtoEmployee> GetEmployees()
        {
            MalaiContext ctx = new MalaiContext(Globals.ConnectionString);
            ctx.GetAllEmployees();
            return ctx.LstEmployee;
        }

        public async Task<List<DtoClient>?> GetClientsAsync()
        {
            await Task.Delay(1);
            return GetClients();
        }

        public List<DtoClient> GetClients()
        {
            MalaiContext ctx = new MalaiContext(Globals.ConnectionString);
            ctx.GetAllClients();
            return ctx.LstClients;
        }

        public async Task<List<DtoJob>?> GetJobsAsync()
        {
            await Task.Delay(1);
            return GetJobs();
        }

        public List<DtoJob> GetJobs()
        {
            MalaiContext ctx = new MalaiContext(Globals.ConnectionString);
            ctx.GetAllJobs();
            return ctx.LstJobs;
        }

        public async Task<List<DtoWorkedHours>?> GetWorkedHoursAsync()
        {
            await Task.Delay(1);
            return GetWorkedHours();
        }
        public List<DtoWorkedHours> GetWorkedHours()
        {
            MalaiContext ctx = new MalaiContext(Globals.ConnectionString);
            ctx.GetAllWorkedHours(DateTime.ParseExact(Globals.SelectedMonth, "MMMM", System.Globalization.CultureInfo.CurrentCulture).Month, Globals.SelectedYear);
            return ctx.LstWorkedHours;
        }
    }
}
