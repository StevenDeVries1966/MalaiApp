using DataLayer.Classes;
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
        Task<List<DtoJob>?> GetJobsAsync();
        Task<List<DtoWorkedHours>?> GetWorkedHoursAsync();
    }

    public class MalaiDataProvider : IMalaiDataProvider
    {
        public async Task<MalaiContext?> GetDataContextAsync()
        {
            await Task.Delay(1);
            MalaiContext ctx = new MalaiContext(Globals.Server, Globals.Database, Globals.Username, Globals.Password, true);
            return ctx;
        }
        public async Task<List<DtoEmployee>?> GetEmployeesAsync()
        {
            await Task.Delay(1);
            return GetEmployees();
        }

        public List<DtoEmployee> GetEmployees()
        {
            MalaiContext ctx = new MalaiContext(Globals.Server, Globals.Database, Globals.Username, Globals.Password);
            ctx.GetAllEmployees();
            return ctx.lstEmployee;
        }

        public async Task<List<DtoClient>?> GetClientsAsync()
        {
            await Task.Delay(1);
            MalaiContext ctx = new MalaiContext(Globals.Server, Globals.Database, Globals.Username, Globals.Password);
            ctx.GetAllClients();
            return ctx.lstClients;
        }

        public async Task<List<DtoJob>?> GetJobsAsync()
        {
            await Task.Delay(1);
            MalaiContext ctx = new MalaiContext(Globals.Server, Globals.Database, Globals.Username, Globals.Password);
            ctx.GetAllJobs();
            return ctx.lstJobs;
        }

        public async Task<List<DtoWorkedHours>?> GetWorkedHoursAsync()
        {
            await Task.Delay(1);
            MalaiContext ctx = new MalaiContext(Globals.Server, Globals.Database, Globals.Username, Globals.Password);
            ctx.GetAllWorkedHours();
            return ctx.lstWorkedHours;
        }
    }
}
